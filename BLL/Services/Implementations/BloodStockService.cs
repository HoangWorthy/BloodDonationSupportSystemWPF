using DAL.Repositories.Implementations;
using DAL.Entities;


namespace BLL.Services.Implementations
{
    public class BloodStockService 
    {
        private readonly BloodStockRepository _bloodStockRepository;

        public BloodStockService()
        {
            _bloodStockRepository = new BloodStockRepository();
        }

        public async Task<List<BloodStock>> GetAllAsync()
        {
            return await _bloodStockRepository.GetAllAsync();
        }

        public async Task<BloodStock?> GetByIdAsync(int id)
        {
            return await _bloodStockRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(BloodStock stock)
        {
            await _bloodStockRepository.AddAsync(stock);
        }

        public async Task UpdateAsync(BloodStock stock)
        {
            await _bloodStockRepository.UpdateAsync(stock);
        }

        public async Task DeleteAsync(int id)
        {
            await _bloodStockRepository.DeleteAsync(id);
        }
    }
}
