using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implementations
{
    public class BloodStockRepository
    {
        private readonly BloodDonationSupportSystemContext _context;

        public BloodStockRepository()
        {
            _context = new BloodDonationSupportSystemContext(); 
        }

        public async Task<List<BloodStock>> GetAllAsync()
        {
            return await _context.BloodStocks.ToListAsync();
        }

        public async Task<BloodStock?> GetByIdAsync(int id)
        {
            return await _context.BloodStocks.FindAsync(id);
        }

        public async Task AddAsync(BloodStock stock)
        {
            _context.BloodStocks.Add(stock);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BloodStock stock)
        {
            _context.BloodStocks.Update(stock);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var stock = await _context.BloodStocks.FindAsync(id);
            if (stock != null)
            {
                _context.BloodStocks.Remove(stock);
                await _context.SaveChangesAsync();
            }
        }
    }
}
