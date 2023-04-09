using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.ProfileExceptions;


using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Storage;

public class InMemoryStore
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
    public Task <Account?> GetAccount(string email){
        if (!accountDict.ContainsKey(email))
        {
            throw new ProfileNotFoundException();
        }
        return Task.FromResult<Account?>(accountDict[email]);
    }

    public Task DeleteAccount(string email){
        if (!accountDict.ContainsKey(email))
        {
            throw new ProfileNotFoundException();
        }
        accountDict.Remove(email);
        return Task.CompletedTask;
    }

    public Task ReplaceAccount(Account account)
    {
        if (!accountDict.ContainsKey(account.email))
        {
            accountDict[account.email] = account;
            throw new ProfileNotFoundException();
        }
        accountDict[account.email] = account;
        return Task.CompletedTask;
    }
}