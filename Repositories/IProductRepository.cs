using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;

namespace Shop_Backend.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task SaveChangesAsync();
    }
}