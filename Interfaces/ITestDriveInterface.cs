using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Interfaces;

public interface ITestDriveInterface
{
    Task AddTestDrive(TestDrive test_drive);
    Task DeleteTestDrive(int Id);
    Task<TestDrive> GetTestDriveByTestDriveId(int Id);
    Task<List<TestDrive>> GetAllTestDriveByCarName(string CarName);
    Task<List<TestDrive>> GetAllTestDriveByAccountEmail(string Email);
    Task<List<TestDrive>> GetAllTestDrives();
    Task<List<long>> GetAllAvailableSlots(string CarName);
    Task<string> GetAccount(int Id);
}