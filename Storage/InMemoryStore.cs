using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.DTOs;
<<<<<<< HEAD
using CarWebsiteBackend.Exceptions;

=======
using Microsoft.AspNetCore.Mvc;
>>>>>>> origin/AccountGet
namespace CarWebsiteBackend.Storage;

public class InMemoryStore : IAccountInterface
{
    private readonly Dictionary<string, Account> accountDict = new();

    public Task AddAccount(Account account)
    {
        if (!accountDict.ContainsKey(account.email))
        {
            accountDict[account.email] = account;
            return Task.CompletedTask;
        }

        throw new ProfileAlreadyExistsException();
    }
    public Task <Account?> GetAccount(Email email){
        if (!accountDict.ContainsKey(email))
        {
            throw new ArgumentException("This email doesn't exist!");
        }
        return Task.FromResult<Account?>(accountDict[email]);
    }

    public Task DeleteAccount(Email email){
        if (!accountDict.ContainsKey(email))
        {
            throw new ArgumentException("This email doesn't exist!");
        }
        accountDict.Remove(email);
        return Task.CompletedTask;
    }
}