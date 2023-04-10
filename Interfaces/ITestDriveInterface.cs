using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface ITestDriveInterface
{
    Task AddTestDrive(TestDrive test_drive);            // Used for adding a TestDrive, returns error if TestDrive already requasted
    Task DeleteTestDrive(int Id);
    Task<TestDrive> GetTestDriveByTestDriveId(int Id);
    Task<List<TestDrive>> GetTestDriveByCarId(int Car_Id);
    Task<List<TestDrive>> GetTestDriveByAccount(int AccountId);
}
