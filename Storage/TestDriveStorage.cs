using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarWebsiteBackend.Storage
{
    public class TestDriveStorage : ITestDrive
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
                           IF NOT EXISTS(SELECT 1 FROM TestDrives WHERE CarId = {test_drive.CarId} AND AccountId = {test_drive.AccountId} AND Time <> {test_drive.Time})
                           AND (SELECT COUNT(*) FROM TestDrives WHERE Time = {test_drive.Time}) < 2
                           AND NOT EXISTS(SELECT 1 FROM TestDrives WHERE CarId = {test_drive.CarId} AND Time = {test_drive.Time})
                           BEGIN
                               INSERT INTO TestDrives (Time, CarId, AccountId) VALUES ({test_drive.Time}, {test_drive.CarId}, {test_drive.AccountId});
                           END
                           COMMIT TRANSACTION;";
                    await _context.Database.ExecuteSqlRawAsync(query);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }


        public Task DeleteTestDrive(string test_drive_id)
        {
            throw new NotImplementedException();
        }

        public Task EditTestDrive(TestDrive test_drive)
        {
            throw new NotImplementedException();
        }

        public Task<List<TestDrive>> GetTestDriveByCarId(string car_id)
        {
            throw new NotImplementedException();
        }

        public Task<TestDrive> GetTestDriveByTestDriveId(string test_drive_id)
        {
            throw new NotImplementedException();
        }
    }
}
    //public class TestDriveStorage : ITestDriveInterface
    //{
    //    private readonly DataContext _context;
    //    public TestDriveStorage(DataContext context)
    //    {
    //        _context = context;
    //    }

    //    public async Task AddTestDrive(TestDrive test_drive)
    //    {
    //        try
    //        {
    //            _context.Add(test_drive);
    //            var saved = await _context.SaveChangesAsync();
    //        }
    //        catch
    //        {
    //            throw new DuplicateCarException();
    //        }
    //    }

    //    public async Task DeleteTestDrive(int Id)
    //    {
            
    //    }

    //    public async Task<TestDrive> GetTestDriveByTestDriveId(int Id)
    //    {

    //    }

    //    public async Task<List<TestDrive>> GetTestDriveByCarId(int Car_Id)
    //    {

    //    }
    //    public async Task<List<TestDrive>> GetTestDriveByAccount(int AccountId)
    //    {

    //    }

    //}
