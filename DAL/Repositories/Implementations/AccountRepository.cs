﻿using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BloodDonationSupportSystemContext _context;

        public AccountRepository()
        {
            _context = new BloodDonationSupportSystemContext();
        }

        public Account? GetByEmailAndPassword(string email, string password)
        {
            return _context.Accounts.FirstOrDefault(a => a.Email == email && a.Password == password);
        }

        public bool EmailExists(string email)
        {
            return _context.Accounts.Any(a => a.Email == email);
        }

        public void Add(Account account)
        {
            _context.Accounts.Add(account);
        }

        public List<Account> GetAccounts()
        {
            return _context.Accounts.ToList();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
