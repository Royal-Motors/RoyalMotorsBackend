using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
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
                    // Check conditions and insert new TestDrive record within a transaction
                    var query = $@"BEGIN TRANSACTION;
                           IF NOT EXISTS(SELECT 1 FROM TestDrives WHERE AccountId = {test_drive.AccountId} AND Time = {test_drive.Time})
                           AND NOT EXISTS(SELECT 1 FROM TestDrives WHERE CarId = {test_drive.CarId} AND Time = {test_drive.Time})
                           AND NOT EXISTS(SELECT 1 FROM TestDrives WHERE Time = {test_drive.Time} AND CarId IN (SELECT CarId FROM TestDrives WHERE AccountId = {test_drive.AccountId}))
                           BEGIN
                               INSERT INTO TestDrives (Time, CarId, AccountId) VALUES ({test_drive.Time}, {test_drive.CarId}, {test_drive.AccountId});
                           END
                           COMMIT TRANSACTION;";
                    var rowsAffected = await _context.Database.ExecuteSqlRawAsync(query);
                    if(rowsAffected < 1)
                    {
                        throw new TestDriveConflictException();
                    }
                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw new InvalidTestDriveRequestException();
                }
            }
        }

        public async Task<List<TestDrive>> GetAllTestDrives()
        {
            return await _context.TestDrives.ToListAsync();
        }


        public Task DeleteTestDrive(string test_drive_id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteTestDrive(int Id)
        {
            throw new NotImplementedException();
        }

        public Task EditTestDrive(TestDrive test_drive)
        {
            throw new NotImplementedException();
        }

        public Task<List<TestDrive>> GetAllTestDriveByAccount(int AccountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<TestDrive>> GetAllTestDriveByCarId(string car_id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TestDrive>> GetAllTestDriveByCarId(int Car_Id)
        {
            throw new NotImplementedException();
        }

        public Task<TestDrive> GetAllTestDriveByTestDriveId(string test_drive_id)
        {
            throw new NotImplementedException();
        }

        public Task<TestDrive> GetTestDriveByTestDriveId(int Id)
        {
            throw new NotImplementedException();
        }
    }
    
}
