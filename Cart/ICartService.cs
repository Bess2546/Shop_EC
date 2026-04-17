using Shop_Backend.DTOs;

namespace Shop_Backend.CartService
{
    public interface ICartService
    {
        Task<CartResponse?> GetCartByUserIdAsync(int userId);
        Task<CartResponse> AddToCartAsync(int userId, AddToCartRequest request);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
        Task<CartResponse?> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity);
    }
}