using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface ITestDriveInterface
{
    Task AddTestDrive(TestDrive test_drive);            // Used for adding a TestDrive, returns error if TestDrive already requasted
    Task DeleteTestDrive(int Id);
    Task<TestDrive> GetTestDriveByTestDriveId(int Id);
    Task<List<TestDrive>> GetAllTestDriveByCarName(string CarName);
    Task<List<TestDrive>> GetAllTestDriveByAccountEmail(string Email);
    Task<List<TestDrive>> GetAllTestDrives();
}
