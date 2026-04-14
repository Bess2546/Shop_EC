using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.DTOs;

namespace Shop_Backend.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllProductAsync();
        Task<ProductResponse?> GetProductByIdAsync(int id);
        Task<ProductResponse> CreateProductAsync(CreateProductRequest request);
        Task<bool> UpdateProductAsync(int id, CreateProductRequest request);
        Task<bool> DeleteProductAsync(int id);
    }
}