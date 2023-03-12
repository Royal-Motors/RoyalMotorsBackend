using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Interfaces;

public interface IAccountInterface
{
    //Task UpsertAccount(Account account); // forces upsert, used for Edit Account
    Task AddAccount(Account account); // used for Sign Up, returns error if account already exists
    //Task<Account?> GetAccount(Email email);
    //Task DeleteAccount(Email email);
}
