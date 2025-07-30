using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class AccountRepository
    {
        private readonly BloodDonationSupportSystemContext _context;

        public AccountRepository()
        {
            _context = new BloodDonationSupportSystemContext();
        }

        public async Task<IEnumerable<Account>> GetAllAsync()
        {
            return await _context.Set<Account>().ToListAsync();
        }

        public async Task<Account?> GetByIdAsync(long id)
        {
            return await _context.Set<Account>()
                .Include(a => a.Profile)
                .Include(a => a.Blogs)
                .Include(a => a.BloodUnits)
                .Include(a => a.DonationEvents)
                .Include(a => a.EventRegistrations)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAsync(Account account)
        {
            await _context.Set<Account>().AddAsync(account);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Account account)
        {
            _context.Set<Account>().Update(account);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var account = await _context.Set<Account>().FindAsync(id);
            if (account != null)
            {
                _context.Set<Account>().Remove(account);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Account?> GetByEmailAsync(string email)
        {
            return await _context.Set<Account>()
                .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
