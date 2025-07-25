using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ComponentRequestRepository
    {
        private readonly BloodDonationSupportSystemContext _context;

        public ComponentRequestRepository()
        {
            _context = new BloodDonationSupportSystemContext();
        }

        public async Task<IEnumerable<ComponentRequest>> GetAllAsync()
        {
            return await _context.ComponentRequests
                .Include(cr => cr.BloodRequests)
                .ToListAsync();
        }

        public async Task<ComponentRequest?> GetByIdAsync(int id)
        {
            return await _context.ComponentRequests
                .Include(cr => cr.BloodRequests)
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }

        public async Task AddAsync(ComponentRequest componentRequest)
        {
            await _context.ComponentRequests.AddAsync(componentRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ComponentRequest componentRequest)
        {
            _context.ComponentRequests.Update(componentRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var componentRequest = await _context.ComponentRequests.FindAsync(id);
            if (componentRequest != null)
            {
                _context.ComponentRequests.Remove(componentRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
