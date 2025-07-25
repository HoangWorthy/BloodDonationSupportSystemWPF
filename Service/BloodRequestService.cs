
using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Repository;

namespace Services
{
    public class BloodRequestService
    {
        private readonly BloodRequestRepository _repository;

        public BloodRequestService()
        {
            _repository = new BloodRequestRepository();
        }

        public Task<IEnumerable<BloodRequest>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<BloodRequest?> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(BloodRequest bloodRequest)
        {
            return _repository.AddAsync(bloodRequest);
        }

        public Task UpdateAsync(BloodRequest bloodRequest)
        {
            return _repository.UpdateAsync(bloodRequest);
        }

        public Task DeleteAsync(long id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
