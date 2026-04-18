using Shop_Backend.DTOs;
using Shop_Backend.Models;
using Shop_Backend.Repositories;

namespace Shop_Backend.OrdersService
{
    public class OrderService : IOrderService
    {
        private const string StatusPending = "Pending";
        private const string StatusCancelled = "Cancelled";

        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        private static OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(i => new OrderItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.ProductName ?? "",
                    Price = i.Price,
                    Quantity = i.Quantity,
                    Subtotal = i.Price * i.Quantity,
                    ImageUrl = i.Product?.ImageUrl
                }).ToList(),
                TotalPrice = order.OrderItems.Sum(i => i.Price * i.Quantity)
            };
        }

        public async Task<OrderResponse> CreateOrderAsync(int userId, CreateOrderRequest request)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId)
                ?? throw new InvalidOperationException("ไม่พบตะกร้าสินค้า");

            var selectedItems = cart.Items
                .Where(i => request.CartItemIds.Contains(i.Id))
                .ToList();

            if (selectedItems.Count == 0)
                throw new InvalidOperationException("ไม่พบสินค้าที่เลือกในตะกร้า");

            foreach (var item in selectedItems)
            {
                if (item.Product!.Stock < item.Quantity)
                    throw new InvalidOperationException(
                        $"สินค้า '{item.Product.ProductName}' คงเหลือไม่เพียงพอ");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = StatusPending,
                OrderItems = selectedItems.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product!.Price
                }).ToList()
            };

            await _orderRepository.AddOrderAsync(order);

            foreach (var item in selectedItems)
            {
                item.Product!.Stock -= item.Quantity;
            }

            await _productRepository.SaveChangesAsync();

            foreach (var item in selectedItems)
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }

            return MapToResponse(order);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order is null ? null : MapToResponse(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return orders.Select(MapToResponse);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null) return false;

            if (order.Status != StatusPending) return false;

            order.Status = StatusCancelled;


            foreach (var item in order.OrderItems)
            {
                item.Product!.Stock += item.Quantity;
            }

            await _orderRepository.SaveChangesAsync();
            await _productRepository.SaveChangesAsync();

            return true;
        }
    }
}