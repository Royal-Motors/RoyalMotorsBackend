using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface ITestDrive
{
    Task AddTestDrive(TestDrive test_drive);            // Used for adding a TestDrive, returns error if TestDrive already requasted
    Task DeleteTestDrive(string test_drive_id);
    Task EditTestDrive(TestDrive test_drive);           // Forces upsert, used for Edit TestDrive
    Task<TestDrive> GetTestDriveByTestDriveId(string test_drive_id);
    Task<List<TestDrive>> GetTestDriveByCarId(string car_id);
}
