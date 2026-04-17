using Shop_Backend.DTOs;


namespace Shop_Backend.StoreServices
{
    public interface IStoreService
    {
        Task<IEnumerable<StoreResponse>> GetAllStoresAsync();
        Task<StoreResponse?> GetStoreByIdAsync(int id);
        Task<StoreResponse> CreateStoreAsync(int ownerId, CreateStoreRequest request);
        Task<bool> UpdateStoreAsync(int id, UpdateStoreRequest request);
        Task<bool> DeleteStoreAsync(int id);
    }
}