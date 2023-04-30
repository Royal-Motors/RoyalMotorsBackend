using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace CarWebsiteBackend.Storage
{
    public class AccountStorage : IAccountInterface
    {

        private readonly DataContext _context;
        public AccountStorage(DataContext context)
        {
            _context = context;
        }

        public async Task AddAccount(Account account)
        {
            try
            {
                _context.Add(account);
                var saved = await _context.SaveChangesAsync();
            }
            catch
            {
                throw new ProfileAlreadyExistsException();
            }
        }

        public async Task DeleteAccount(string email)
        {   
            var sql = "DELETE FROM Accounts WHERE email = @Email";
            var parameters = new[] { new SqlParameter("@Email", email) };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new ProfileNotFoundException();
            }
        }

        public async Task<Account> GetAccount(string email)
        {
            var Acc = await _context.Accounts.Where(p => p.email == email).FirstOrDefaultAsync();
            if (Acc == null)
            {
                throw new ProfileNotFoundException();
            }
            return Acc;
        }

        public async Task ReplaceAccount(Account account)
        {
            var sql = "UPDATE Accounts SET firstname = @NewFirstname, lastname = @NewLastname, password = @NewPassword WHERE email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@NewFirstname", account.firstname),
            new SqlParameter("@NewLastname", account.lastname),
            new SqlParameter("@NewPassword", account.password),
            new SqlParameter("@Email", account.email)
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if(rowsAffected <= 0)
            {
                throw new ProfileNotFoundException();
            }
        }

        public async Task VerifyAccount(string email)
        {
            var sql = "UPDATE Accounts SET verificationCode = @NewVerificationCode, verified = @NewVerified WHERE email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@NewVerified", true),
            new SqlParameter("@NewVerificationCode", ""),
            new SqlParameter("@Email", email)
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new ProfileNotFoundException();
            }
        }

        public async Task DeleteUnverifiedAccounts()
        {
            DateTime cutoffDate = DateTime.UtcNow.AddHours(-24);
            string sql = @"DELETE FROM Accounts WHERE verified = false AND CreationDate < {0}";
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, cutoffDate);

        }
    }
}