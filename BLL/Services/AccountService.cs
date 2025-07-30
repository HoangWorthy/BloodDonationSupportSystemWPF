using DAL.Entities;
using DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly AccountRepository _accountRepository;

        public AccountService()
        {
            _accountRepository =  new AccountRepository();
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _accountRepository.GetAllAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(long id)
        {
            return await _accountRepository.GetByIdAsync(id);
        }

        public async Task<Account?> GetAccountByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task AddAccountAsync(Account account)
        {
            await _accountRepository.AddAsync(account);
        }

        public async Task UpdateAccountAsync(Account account)
        {
            await _accountRepository.UpdateAsync(account);
        }

        public async Task DeleteAccountAsync(long id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
