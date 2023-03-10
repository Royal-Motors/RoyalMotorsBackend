using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Interfaces;

public interface IAccountInterface
{
    Task UpsertAccount(Account account);
    Task<Account?> GetAccount(Email email);
    Task DeleteAccount(Email email);
}
