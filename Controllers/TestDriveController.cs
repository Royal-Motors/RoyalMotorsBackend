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

    [HttpGet("c/{Car_Id}")]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByCarId(int Car_Id)
    {
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByCarId(Car_Id);
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

    [HttpGet("a/{AccountId}")]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByAccount(int AccountId)
    {
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByAccount(AccountId);
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