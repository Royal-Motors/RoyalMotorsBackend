﻿using CarWebsiteBackend.Data;
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

        public async Task PutForgotPasswordCode(string email, string code)
        {
            var sql = "UPDATE Accounts SET verificationCode = @Code WHERE email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@Code", code),
            new SqlParameter("@Email", email)
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new ProfileNotFoundException();
            }
        }

        public async Task ReplaceAccount(Account account)
        {
            var sql = "UPDATE Accounts SET firstname = @NewFirstname, lastname = @NewLastname, password = @NewPassword," +
                "address = @NewAddress, phoneNumber = @NewPhoneNumber WHERE email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@NewFirstname", account.firstname),
            new SqlParameter("@NewLastname", account.lastname),
            new SqlParameter("@NewPassword", account.password),
            new SqlParameter("@Email", account.email),
            new SqlParameter("@NewAddress", account.address),
            new SqlParameter("@NewPhoneNumber", account.phoneNumber)
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if(rowsAffected <= 0)
            {
                throw new ProfileNotFoundException();
            }
        }

        public async Task ResetPassword(string email, string password, string code)
        {
            var sql = "UPDATE Accounts SET password = @NewPassword, verificationCode = @EmptyCode WHERE verificationCode = @Code AND email = @Email";
            var parameters = new[]
            {
            new SqlParameter("@Code", code),
            new SqlParameter("@Email", email),
            new SqlParameter("@NewPassword", password),
            new SqlParameter("@EmptyCode", "")
            };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
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
    }
}