using DAL.Entities;

namespace DAL.Repositories
{
    public interface IAccountRepository
    {
        Account? GetByEmailAndPassword(string email, string password);
        bool EmailExists(string email);
        void Add(Account account);
        void Save();
    }
}
