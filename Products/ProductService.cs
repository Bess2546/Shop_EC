using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;
using Shop_Backend.Models;
using Shop_Backend.Repositories;

namespace Shop_Backend.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Price = product.Price,
                Stock = product.Stock,
                StoreId = product.StoreId,
                ImageUrl = product.ImageUrl

            };
        }

        public async Task<IEnumerable<ProductResponse>> GetAllProductAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToResponse);
        }

        public async Task<ProductResponse?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;

            return MapToResponse(product);
        }

        public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Stock = request.Stock,
                StoreId = request.StoreId,
                ImageUrl = request.ImageUrl
            };

            await _repository.AddAsync(product);
            return MapToResponse(product);
        }

        public async Task<bool> UpdateProductAsync(int id, CreateProductRequest request)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            product.ProductName = request.ProductName;
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.StoreId = request.StoreId;

            await _repository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return false;

            await _repository.DeleteAsync(product);
            return true;
        }
    }
}