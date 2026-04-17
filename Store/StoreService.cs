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

        private static StoreResponse MapToResponse(Store store)
        {
            return new StoreResponse
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
                OwnerId = store.OwnerId
            };
        }

        public async Task<IEnumerable<StoreResponse>> GetAllStoresAsync()
        {
            var stores = await _repository.GetAllAsync();
            return stores.Select(MapToResponse);
        }

        public async Task<StoreResponse?> GetStoreByIdAsync(int id)
        {
            var store = await _repository.GetByIdAsync(id);
            return store is null ? null : MapToResponse(store);
        }

        public async Task<StoreResponse> CreateStoreAsync(int ownerId, CreateStoreRequest request)
        {
            var store = new Store
            {
                Name = request.Name,
                Description = request.Description,
                OwnerId = ownerId
            };

            await _repository.AddAsync(store);
            return MapToResponse(store);
        }

        public async Task<bool> UpdateStoreAsync(int id, UpdateStoreRequest request)
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