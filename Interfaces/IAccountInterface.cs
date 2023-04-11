using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface IAccountInterface
{
    Task ReplaceAccount(Account account); // forces upsert, used for Edit Account
    Task AddAccount(Account account); // used for Sign Up, returns error if account already exists
    Task <Account> GetAccount(string email);
    Task DeleteAccount(string email);
    Task VerifyAccount(string email);
}
