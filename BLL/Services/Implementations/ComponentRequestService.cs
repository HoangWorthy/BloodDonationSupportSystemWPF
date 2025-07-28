using DAL.Entities;
using Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ComponentRequestService
    {
        private readonly Repository.ComponentRequestRepository _repository;

        public ComponentRequestService()
        {
            _repository = new Repository.ComponentRequestRepository();
        }

        public Task<IEnumerable<ComponentRequest>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<ComponentRequest?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(ComponentRequest componentRequest)
        {
            return _repository.AddAsync(componentRequest);
        }

        public Task UpdateAsync(ComponentRequest componentRequest)
        {
            return _repository.UpdateAsync(componentRequest);
        }

        public Task DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
