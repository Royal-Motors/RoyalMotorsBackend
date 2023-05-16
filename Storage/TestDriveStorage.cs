using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace CarWebsiteBackend.Storage;

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

    public async Task<List<long>> GetAllAvailableSlots(string CarName)
    {
        var testDrives = await _context.TestDrives
            .Include(td => td.Car)
            .Include(td => td.Account)
            .ToListAsync();
        var testDrivesCar = await _context.TestDrives
            .Include(td => td.Account)
            .Where(td => td.Car.name == CarName)
            .ToListAsync();
        var slots = timeSlotGenerator();
        foreach (var test_drive in testDrives)
        {
            if (slots.Contains(test_drive.Time))
            {
                slots.Remove(test_drive.Time);
            }
        }
        foreach (var test_drive_car in testDrivesCar)
        {
            if (slots.Contains(test_drive_car.Time))
            {
                slots.Remove(test_drive_car.Time);
            }
        }
        List<long> uniqueSlots = slots.GroupBy(x => x).Select(x => x.Key).ToList();
        return uniqueSlots;
    }

    public static List<long> timeSlotGenerator()
    {
        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

        // Create an empty list to hold the time slots
        List<long> timeSlots = new List<long>();

        // Loop through the upcoming week
        for (int i = 0; i < 7; i++)
        {
            // Get the current date in the local time zone
            DateTime currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(i + 1).Date, timeZone);
            // Set the start time to the beginning of next day at 10am in the local time zone
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(i + 1).Date.AddHours(6), timeZone);
            // Set the end time to 5pm in the local time zone
            DateTime endTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(i + 1).Date.AddHours(13), timeZone);

            // Check if the current day is not a weekend day (Saturday or Sunday)
            if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
            {

                // Loop through the time slots from the start time to the end time with a 1-hour step
                for (DateTime currentSlot = startTime; currentSlot < endTime; currentSlot = currentSlot.AddHours(1))
                {
                    // Convert the current time slot to Unix time
                    long unixTime = (long)(currentSlot.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                    // Add the Unix time to the list of time slots
                    timeSlots.Add(unixTime);
                    timeSlots.Add(unixTime);
                }
            }
        }
        // Convert the list of time slots to an array
        long[] timeSlotsArray = timeSlots.ToArray();

        return timeSlots;
    }
        
    public async Task<string> GetAccount(int Id)
    {
        var testDrive = await _context.TestDrives.Where(p => p.Id == Id)
            .Include(td => td.Car)
            .Include(td => td.Account)
            .FirstOrDefaultAsync();
        if (testDrive == null)
        {
            throw new TestDriveNotFoundException();
        }
        return testDrive.Account.email;
    }

    public async Task<string> GetAccountFirstname(int Id)
    {
        var testDrive = await _context.TestDrives.Where(p => p.Id == Id)
            .Include(td => td.Car)
            .Include(td => td.Account)
            .FirstOrDefaultAsync();
        if (testDrive == null)
        {
            throw new TestDriveNotFoundException();
        }
        return testDrive.Account.firstname;
    }

    public async Task<string> GetAccountLastname(int Id)
    {
        var testDrive = await _context.TestDrives.Where(p => p.Id == Id)
            .Include(td => td.Car)
            .Include(td => td.Account)
            .FirstOrDefaultAsync();
        if (testDrive == null)
        {
            throw new TestDriveNotFoundException();
        }
        return testDrive.Account.lastname;
    }
}
