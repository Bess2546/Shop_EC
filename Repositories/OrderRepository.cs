using Shop_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Shop_Backend.Models;

namespace Shop_Backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async  Task AddOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order?> GetByIdAsync (int id)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();
        
    }
}