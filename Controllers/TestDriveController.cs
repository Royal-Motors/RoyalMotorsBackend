using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("test_drive")]
public class TestDriveController : ControllerBase
{
    private readonly ITestDriveInterface testdriveInterface;
    private readonly IAccountInterface accountInterface;
    private readonly CarInterface carInterface;

    public TestDriveController(ITestDriveInterface testdriveInterface, IAccountInterface accountInterface, CarInterface carInterface)
    {
        this.testdriveInterface = testdriveInterface;
        this.accountInterface = accountInterface;
        this.carInterface = carInterface;
    }

    [HttpPost]
    public async Task<ActionResult<TestDrive>> AddTestDrive(CreateTestDrive create_test_drive)
    {
        try
        {
            var account = await accountInterface.GetAccount(create_test_drive.Email);
            var car = await carInterface.GetCar(create_test_drive.CarName);

            var test_drive = new TestDrive(create_test_drive.Time, car, account);

            await testdriveInterface.AddTestDrive(test_drive);
            return CreatedAtAction(nameof(AddTestDrive), new { create_test_drive = create_test_drive }, create_test_drive);
        }
        catch (Exception e)
        {
            if (e is TestDriveConflictException)
            {
                return Conflict($"Cannot schedule a test drive for user {create_test_drive.Email}" +
                    $" with car {create_test_drive.CarName} at time {create_test_drive.Time} due to conflict.");
            }
            else if (e is InvalidTestDriveRequestException)
            {
                return BadRequest("Invalid test drive request.");
            }
            else if(e is ProfileNotFoundException)
            {
                return NotFound($"Cannot schedule a test drive for non-existing user: {create_test_drive.Email}");
            }
            else if(e is CarNotFoundException)
            {
                return NotFound($"Cannot schedule a test drive for a non-existing car {create_test_drive.CarName}");
            }
            throw;
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

    [HttpGet]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDrives()
    {
        try
        {
            var testDrives = await testdriveInterface.GetAllTestDrives();
            return Ok(testDrives);
        }
        catch(Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"No test drives found");
            }
            throw;
        }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteAllTestDrives()
    {
        await testdriveInterface.DeleteAllTestDrives();
        return Ok();
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

    [HttpGet("{Car_Id}")]
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

    [HttpGet("{AccountId}")]
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