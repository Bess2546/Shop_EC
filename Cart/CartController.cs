using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.CartService;
using Shop_Backend.Controllers;
using Shop_Backend.DTOs;


namespace Shop_Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : BaseController
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = GetCurrentUserId();
            var cart = await _service.GetCartByUserIdAsync(userId);
            if (cart is null) return NotFound();
            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddToCartRequest request)
        {
            var userId = GetCurrentUserId();
            var cart = await _service.AddToCartAsync(userId, request);
            return Ok(cart);
        }

        [HttpDelete("items/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var userId = GetCurrentUserId();
            var result = await _service.RemoveFromCartAsync(userId, cartItemId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("items/{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, UpdateCartItemRequest request)
        {
            var userId = GetCurrentUserId();
            var cart = await _service.UpdateCartItemQuantityAsync(userId, cartItemId, request.Quantity!.Value);
            if (cart is null) return NotFound();
            return Ok(cart);
        }


        
    }
}