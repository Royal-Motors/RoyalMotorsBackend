using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using CarWebsiteBackend.Exceptions;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("test_drive")]
public class TestDriveController : ControllerBase
{
    private readonly ITestDriveInterface testdriveInterface;

    public TestDriveController(ITestDriveInterface testdriveInterface)
    {
        this.testdriveInterface = testdriveInterface;
    }

    [HttpPost]
    public async Task<ActionResult<TestDrive>> AddTestDrive(TestDrive create_test_drive)
    {
        try
        {
            var test_drive = new TestDrive(create_test_drive.Time, create_test_drive.CarId, create_test_drive.AccountId);
            await testdriveInterface.AddTestDrive(test_drive);
            return CreatedAtAction(nameof(AddTestDrive), new { CarId = test_drive.CarId }, test_drive);
        }
        catch (Exception e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteTestDrive(int Id)
    {

        try
        {
            await testdriveInterface.DeleteTestDrive(Id);
            return Ok("TestDrive successfully deleted");
        }

        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"TestDrive with Id {Id} not found.");
            }
            throw;
        }

    }

    [HttpGet("{Id}")]
    public async Task<ActionResult<TestDrive>> GetTestDriveByTestDriveId(int Id)
    {

        //not really needed since this won't be called unless a user is signed in and opened his account
        try
        {
            var test_drive = await testdriveInterface.GetTestDriveByTestDriveId(Id);
            return Ok(test_drive);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"TestDrive with Id {Id} not found.");
            }
            throw;
        }
    }

    [HttpGet("car/{Car_Name}")]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByCarName(string Car_Name)
    {
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByCarName(Car_Name);
            return Ok(test_drives);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"No TestDrive found.");
            }
            throw;
        }

    }

    [HttpGet("account/{Account_Email}")]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByAccountEmail(string Account_Email)
    {
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByAccountEmail(Account_Email);
            return Ok(test_drives);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"No TestDrive found.");
            }
            throw;
        }

    }
}