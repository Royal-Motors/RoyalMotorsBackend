using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Storage;

public class InMemoryStore : IAccountInterface
{
    private readonly Dictionary<Email, Account> accountDict = new();

    public Task AddAccount(Account account)
    {
        if (!accountDict.ContainsKey(account.email))
        {
            accountDict[account.email] = account;
            return Task.CompletedTask;
        }

        throw new ArgumentException("Email already taken. Sign up with another email, or sign in.");
    }
}