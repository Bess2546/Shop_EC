using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;

namespace Shop_Backend.OrdersService
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderResponse?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderResponse>> GetOrdersByUserIdAsync(int userId);
        Task<bool> CancelOrderAsync(int orderId);
    }
}