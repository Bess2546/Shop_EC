using Shop_Backend.DTOs;

namespace Shop_Backend.OrdersService
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(int userId, CreateOrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<bool> CancelOrderAsync(int orderId);
    }
}