using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace CarWebsiteBackend.Storage
{
    public class TestDriveStorage : ITestDriveInterface
    {
        private readonly DataContext _context;

        public TestDriveStorage(DataContext context)
        {
            _context = context;
        }

        private int UpToOneHour(int unixTime)
        {
            return unixTime + 3600;
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

                    var testDriveEndTime = UpToOneHour(test_drive.Time);

                    if (existingTestDrives.Any(td=> td.AccountId == test_drive.AccountId && td.CarId == test_drive.CarId))
                    {
                        throw new TestDriveConflictException("Test drive already exists for this car and account.");
                    }

                    if (existingTestDrives.Any(td => td.AccountId == test_drive.AccountId && td.Time < testDriveEndTime && UpToOneHour(td.Time) > test_drive.Time))
                    {
                        throw new TestDriveConflictException("User cannot have two test drives overlapping in time.");
                    }

                    if (existingTestDrives.Count(td => td.Time == test_drive.Time) >= 2)
                    {
                        throw new TestDriveConflictException("Two test drives already exist for this time.");
                    }

                    if (existingTestDrives.Any(td => td.CarId == test_drive.CarId && td.Time < testDriveEndTime && UpToOneHour(td.Time) > test_drive.Time))
                    {
                        throw new TestDriveConflictException("A test drive already exists for this car at this time.");
                    }

                    await _context.TestDrives.AddAsync(test_drive);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteTestDrive(int Id)
        {
            var sql = "DELETE FROM TestDrives WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", Id) };
            var rowsAffected = await _context.Database.ExecuteSqlRawAsync(sql, parameters);
            if (rowsAffected <= 0)
            {
                throw new TestDriveNotFoundException();
            }
            return;
        }

        public async Task<List<TestDrive>> GetAllTestDriveByAccountEmail(string Email)
        {
            var testDrives = await _context.TestDrives
                .Include(td => td.Car)
                .Where(td => td.Account.email == Email)
                .ToListAsync();
            if(testDrives.Count == 0)
            {
                throw new TestDriveNotFoundException();
            }
            return testDrives;
        }

        public async Task<List<TestDrive>> GetAllTestDriveByCarName(string CarName)
        {
            var testDrives = await _context.TestDrives
                .Include(td => td.Account)
                .Where(td => td.Car.name == CarName)
                .ToListAsync();
            if (testDrives.Count == 0)
            {
                throw new TestDriveNotFoundException();
            }
            return testDrives;
        }

        public async Task<TestDrive> GetTestDriveByTestDriveId(int Id)
        {
            var testDrive = await _context.TestDrives.Where(p => p.Id == Id)
                .Include(td => td.Car)
                .Include(td => td.Account)
                .FirstOrDefaultAsync();
            if (testDrive == null)
            {
                throw new TestDriveNotFoundException();
            }
            return testDrive;
        }

        public async Task<List<TestDrive>> GetAllTestDrives()
        {
            var testDrives = await _context.TestDrives
                .Include(td => td.Car)
                .Include(td => td.Account)
                .ToListAsync();
            if (testDrives.Count == 0)
            {
                throw new TestDriveNotFoundException();
            }
            return testDrives;
        }
    }
}
