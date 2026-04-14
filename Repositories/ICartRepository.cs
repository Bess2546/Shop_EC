using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;

namespace Shop_Backend.Repositories
{
    public interface ICartRepository
    {
    Task<Cart?> GetCartByUserIdAsync(int userId);
    Task AddCartAsync(Cart cart);
    Task AddCartItemAsync(CartItem item);
    Task RemoveCartItemAsync(CartItem item);
    Task SaveChangesAsync();   
    }
}