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
            if (cart is null) return null;

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

        public async Task<CartResponse> AddToCartAsync(int userId, AddToCartRequest request)
        {
           
            var productId = request.ProductId!.Value;
            var quantity = request.Quantity!.Value;

            var product = await _productRepository.GetByIdAsync(productId)
                ?? throw new InvalidOperationException("ไม่พบสินค้า");

            if (product.Stock < quantity)
                throw new InvalidOperationException("สินค้าในคลังไม่เพียงพอ");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepository.AddCartAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem is not null)
            {
                existingItem.Quantity += quantity;
                await _cartRepository.SaveChangesAsync();
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                await _cartRepository.AddCartItemAsync(newItem);
            }

            return (await GetCartByUserIdAsync(userId))!;
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart is null) return false;

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return false;

            await _cartRepository.RemoveCartItemAsync(item);
            return true;
        }

        public async Task<CartResponse?> UpdateCartItemQuantityAsync(int userId, int cartItemId, int quantity)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart is null) return null;

            var item = cart.Items.FirstOrDefault(i => i.Id == cartItemId);
            if (item is null) return null;

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