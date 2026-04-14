using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shop_Backend.Models;
using Shop_Backend.DTOs;
using Shop_Backend.Repositories;


namespace Shop_Backend.StoreServices
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _repository;

        public StoreService(IStoreRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<StoreResponse>> GetAllStoresAsync()
        {
            var stores = await _repository.GetAllAsync();
            return stores.Select(s => new StoreResponse
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                OwnerId = s.OwnerId
            });
        }

        public async Task<StoreResponse?> GetStoreByIdAsync(int id)
        {
            var Store = await _repository.GetByIdAsync(id);
            if (Store == null) return null;

            return new StoreResponse
            {
                Id = Store.Id,
                Name = Store.Name,
                Description = Store.Description,
                OwnerId = Store.OwnerId
            };
        }

        public async Task<StoreResponse> CreateStoreAsync(CreateStoreRequest request)
        {
            var Store = new Store
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = request.OwnerId
            };

            await _repository.AddAsync(Store);
            return new StoreResponse
            {
                Id = Store.Id,
                Name = Store.Name,
                Description = Store.Description,
                OwnerId = Store.OwnerId
            };
        }

        public async Task<bool> UpdateStoreAsync(int id, CreateStoreRequest request)
        {
            var store = await _repository.GetByIdAsync(id);
            if (store == null) return false;

            store.Name = request.Name;
            store.Description = request.Description;

            await _repository.UpdateAsync(store);
            return true;
        }

        public async Task<bool> DeleteStoreAsync(int id)
        {
            var store = await _repository.GetByIdAsync(id);
            if (store == null) return false;

            await _repository.DeleteAsync(store);
            return true;
        }
    }
}