using CarWebsiteBackend.Data;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarWebsiteBackend.Exceptions.CarExceptions;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace CarWebsiteBackend.Storage
{
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
}