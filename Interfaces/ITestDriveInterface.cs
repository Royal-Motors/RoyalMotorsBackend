using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Interfaces;

public interface ITestDriveInterface
{
    Task AddTestDrive(TestDrive test_drive);            // Used for adding a TestDrive, returns error if TestDrive already requasted
    Task DeleteTestDrive(int Id);
    Task<TestDrive> GetTestDriveByTestDriveId(int Id);
    Task<List<TestDrive>> GetAllTestDriveByCarName(string CarName);
    Task<List<TestDrive>> GetAllTestDriveByAccountEmail(string Email);
    Task<List<TestDrive>> GetAllTestDrives();
    Task<List<long>> GetAllAvailableSlots(string CarName);
    Task<string> GetAccount(int Id);
    Task<string> GetAccountFirstname(int Id)
    Task<string> GetAccountLastname(int Id)
}
