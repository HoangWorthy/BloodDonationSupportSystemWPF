using DAL.Entities;

namespace BLL.Services
{
    public interface IAccountService
    {
        Account? Login(string email, string password);
        bool Register(Account account);
        bool EmailExists(string email);
        List<Account> GetAccounts();
    }
}