using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.OrdersService;
using Shop_Backend.DTOs;
using Shop_Backend.Controllers;

namespace Shop_Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

   
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var userId = GetCurrentUserId();
            var order = await _service.CreateOrderAsync(userId, request);
            return Ok(order);
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetCurrentUserId();
            var order = await _service.GetOrderByIdAsync(id);
            if (order is null) return NotFound();

            
            if (order.UserId != userId) return Forbid();

            return Ok(order);
        }

        // GET /api/order/me — ดู order ทั้งหมดของตัวเอง
        [HttpGet("me")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetCurrentUserId();
            var orders = await _service.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }
        
        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = GetCurrentUserId();
            var order = await _service.GetOrderByIdAsync(id);
            if (order is null) return NotFound();

            if (order.UserId != userId) return Forbid();

            var result = await _service.CancelOrderAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    
    }
}