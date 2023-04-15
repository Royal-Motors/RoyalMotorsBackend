using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Email;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace CarWebsiteBackend.Storage
{
    public class TestDriveStorage : ITestDriveInterface
    {
        private readonly DataContext _context;
        public TestDriveStorage(DataContext context)
        {
            _context = context;
        }
        public async Task AddTestDrive(TestDrive test_drive)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var existingTestDrives = await _context.TestDrives
                        .FromSqlRaw("SELECT * FROM TestDrives")
                        .ToListAsync();

                    if (existingTestDrives.Any(td=> td.AccountId == test_drive.AccountId && td.CarId == test_drive.CarId))
                    {
                        throw new Exception("Test drive already exists for this car and account with at this time or different time.");
                    }

                    if (existingTestDrives.Any(td => td.AccountId == test_drive.AccountId && td.Time == test_drive.Time))
                    {
                        throw new Exception("User Cannot have two test drives at the same time.");
                    }

                    if (existingTestDrives.Count(td => td.Time == test_drive.Time) >= 2)
                    {
                        throw new Exception("Two test drives already exist for this time.");
                    }

                    if (existingTestDrives.Any(td => td.Time == test_drive.Time && td.CarId == test_drive.CarId))
                    {
                        throw new Exception("A test drive already exists for this car and time.");
                    }

                    await _context.TestDrives.AddAsync(test_drive);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task DeleteTestDrive(int Id)
        {
            var sql = "DELETE FROM Accounts WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", Id) };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new TestDriveNotFoundException();
            }
        }

        public async Task<List<TestDrive>> GetAllTestDriveByAccount(int AccountId)
        {
            var testDrives = await _context.TestDrives.Where(p => p.AccountId == AccountId).ToListAsync();
            return testDrives;
        }

        public async Task<List<TestDrive>> GetAllTestDriveByCarId(int Car_Id)
        {
            var testDrives = await _context.TestDrives.Where(p => p.CarId == Car_Id).ToListAsync();
            return testDrives;
        }

        public async Task<TestDrive> GetTestDriveByTestDriveId(int Id)
        {
            var testDrive = await _context.TestDrives.Where(p => p.Id == Id).FirstOrDefaultAsync();
            return testDrive;
        }
    }
}
