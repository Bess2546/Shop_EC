using Shop_Backend.DTOs;
using Shop_Backend.Models;
using Shop_Backend.OrdersService;
using Shop_Backend.Repositories;

namespace Shop_Backend.OrdersService
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository,ICartRepository cartRepository,IProductRepository productRepository)
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

        public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);
            if (cart == null) throw new Exception("Cart not found");

            var order = new Order
            {
              UserId = request.UserId,
              OrderDate = DateTime.UtcNow,
              Status = "Pending",
              OrderItems = cart.Items.Select(i => new OrderItem
              {
                  ProductId = i.ProductId,
                  Quantity = i.Quantity,
                  Price = i.Product!.Price
              }).ToList()
            };

            await _orderRepository.AddOrderAsync(order);

            foreach (var item in cart.Items)
            {
                item.Product!.Stock -= item.Quantity;
            }

            await _productRepository.SaveChangesAsync();

            foreach (var item in cart.Items.ToList())
            {
                await   _cartRepository.RemoveCartItemAsync(item);
            }

            return MapToResponse(order);
        }

        public async Task<OrderResponse?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return MapToResponse(order);
        }

        public async Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId)
        {
            var order = await _orderRepository.GetByUserIdAsync(userId);
            return order.Select(MapToResponse);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) return false;

            if(order.Status != "Pending") return false;

            order.Status = "Cancelled";

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