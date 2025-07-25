using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class BloodRequestRepository
    {
        private readonly BloodDonationSupportSystemContext _context;
        public BloodRequestRepository()
        {
            _context = new BloodDonationSupportSystemContext();
        }

        public async Task<IEnumerable<BloodRequest>> GetAllAsync()
        {
            return await _context.BloodRequests
                .Include(br => br.BloodUnits)
                .Include(br => br.ComponentRequest)
                .ToListAsync();
        }

        public async Task<BloodRequest?> GetByIdAsync(long id)
        {
            return await _context.BloodRequests
                .Include(br => br.BloodUnits)
                .Include(br => br.ComponentRequest)
                .FirstOrDefaultAsync(br => br.Id == id);
        }

        public async Task AddAsync(BloodRequest bloodRequest)
        {
            await _context.BloodRequests.AddAsync(bloodRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BloodRequest bloodRequest)
        {
            _context.BloodRequests.Update(bloodRequest);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var bloodRequest = await _context.BloodRequests.FindAsync(id);
            if (bloodRequest != null)
            {
                _context.BloodRequests.Remove(bloodRequest);
                await _context.SaveChangesAsync();
            }
        }
    }
}
