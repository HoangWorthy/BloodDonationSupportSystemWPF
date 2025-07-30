using BLL.Services;
using BLL.Services.Implementations;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        public AccountService()
        {
            _repository = new AccountRepository();
        }

        public Account? Login(string email, string password)
        {
            return _repository.GetByEmailAndPassword(email, password);
        }

        public bool Register(Account account)
        {
            if (_repository.EmailExists(account.Email)) return false;
            _repository.Add(account);
            _repository.Save();
            return true;
        }

        public bool EmailExists(string email)
        {
            return _repository.EmailExists(email);
        }

        public List<Account> GetAccounts()
        {
            return _repository.GetAccounts();
        }
    }
}
