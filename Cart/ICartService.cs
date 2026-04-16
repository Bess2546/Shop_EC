using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;

namespace Shop_Backend.CartService
{
    public interface ICartService
    {
        Task<CartResponse?> GetCartByUserIdAsync(int userId);

        Task<CartResponse> AddToCartAsync(AddToCartRequest request);

        Task<bool> RemoveFromCartAsync(int userId, int CartItemId);

        Task<CartResponse?> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity);
    }
}