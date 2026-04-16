
using Shop_Backend.DTOs;
using Shop_Backend.Models;
using Shop_Backend.Repositories;

namespace Shop_Backend.CartService
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartResponse?> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return null;
            return new CartResponse
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemResponse
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product!.ProductName,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,
                    Subtotal = i.Product.Price * i.Quantity,
                    ImageUrl = i.Product.ImageUrl

                }).ToList(),
                TotalPrice = cart.Items.Sum(i => i.Product!.Price * i.Quantity)
            };
        }

        public async Task<CartResponse> AddToCartAsync(AddToCartRequest request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null) throw new Exception("Product not found");

            if (product.Stock < request.Quantity) throw new Exception("Not enough Stock");

            var cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);
            if (cart == null)
            {
                cart = new Cart { UserId = request.UserId };
                await _cartRepository.AddCartAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                await _cartRepository.SaveChangesAsync();
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };

                await _cartRepository.AddCartItemAsync(newItem);
            }
            return (await GetCartByUserIdAsync(request.UserId))!;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null) return false;
            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);

            if (item == null) return false;

            await _cartRepository.RemoveCartItemAsync(item);
            return true;
        }

        public async Task<CartResponse?> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) return null;

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item == null) return null;

            if (quantity <= 0)
            {
                await _cartRepository.RemoveCartItemAsync(item);
            }
            else
            {
                item.Quantity = quantity;
                await _cartRepository.SaveChangesAsync();
            }

            return (await GetCartByUserIdAsync(userId))!;
        }
    }
}