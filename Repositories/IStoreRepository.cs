using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;

namespace Shop_Backend.Repositories
{
    public interface IStoreRepository
    {
        Task<IEnumerable<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
        Task AddAsync(Store store);
        Task UpdateAsync(Store store);
        Task DeleteAsync(Store store);
    }
}