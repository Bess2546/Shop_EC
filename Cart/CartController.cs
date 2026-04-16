using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop_Backend.CartService;
using Shop_Backend.DTOs;


namespace Shop_Backend.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(int userId)
        {
            var cart = await _service.GetCartByUserIdAsync(userId);
            if (cart == null) return NotFound();

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddToCartRequest request)
        {
            var cart = await _service.AddToCartAsync(request);
            return Ok(cart);
        }

        [HttpDelete("{userId}/items/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int userId, int cartItemId)
        {
            var result = await _service.RemoveFromCartAsync(userId, cartItemId);
            if (!result) return NotFound();

            return NoContent();
        }

        [HttpPut("{userId}/items/{cartItemId}")]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int userId, int cartItemId, [FromBody] UpdateCartItemRequest request)
        {
            var cart = await _service.UpdateCartItemQuantityAsync(userId, cartItemId, request.Quantity);
            if (cart == null) return NotFound();
            return Ok(cart);
        }
    }
}