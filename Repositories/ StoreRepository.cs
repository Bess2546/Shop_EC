using Shop_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shop_Backend.Models; 

namespace Shop_Backend.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext _context;

        public StoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Store>> GetAllAsync()
            => await _context.Stores.ToListAsync();

        public async Task<Store?> GetByIdAsync(int id)
            => await _context.Stores.FindAsync(id);

        public async Task AddAsync(Store store)
        {
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Store store)
        {
            _context.Stores.Update(store);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Store store)
        {
            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
        }
    }
}