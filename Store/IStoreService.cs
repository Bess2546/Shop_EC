using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;
using Shop_Backend.DTOs;


namespace Shop_Backend.StoreServices
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreResponse>> GetAllStoresAsync();
        Task<StoreResponse?> GetStoreByIdAsync(int id);
        Task<StoreResponse> CreateStoreAsync(CreateStoreRequest request);
        Task<bool> UpdateStoreAsync(int id, CreateStoreRequest request);
        Task<bool> DeleteStoreAsync(int id);
    }
}