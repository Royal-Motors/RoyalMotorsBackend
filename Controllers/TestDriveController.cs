using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions.TestDriveExceptions;
using CarWebsiteBackend.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("testdrive")]
public class TestDriveController : ControllerBase
{
    private readonly ITestDriveInterface testdriveInterface;
    private readonly IAccountInterface accountStore;
    private readonly CarInterface carStore;
    private readonly IImageInterface imageStore;

    public TestDriveController(ITestDriveInterface testdriveInterface, IAccountInterface accountStore, CarInterface carStore, IImageInterface imageStore)
    {
        this.testdriveInterface = testdriveInterface;
        this.accountStore = accountStore;
        this.carStore = carStore;
        this.imageStore = imageStore;
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<TestDrive>> AddTestDrive(TestDriveRequest request)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if (emailClaim != request.AccountEmail && emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized!");
        }
        if (!request.AccountEmail.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        try
        {
            var account = await accountStore.GetAccount(request.AccountEmail);
            var car = await carStore.GetCar(request.CarName);
            var slots = await testdriveInterface.GetAllAvailableSlots(request.CarName);
            if (!slots.Contains(request.Time)) 
            {
                return Conflict("Time slot is unavailable.");
            }
            TestDrive testDrive = new(request, car, account);
            await testdriveInterface.AddTestDrive(testDrive);

            try
            {
                DateTimeOffset utcDateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(request.Time).ToUniversalTime();
                DateTimeOffset gmtDateTimeOffset = utcDateTimeOffset.AddHours(3);
                string gmtDateString = gmtDateTimeOffset.ToString("yyyy-MM-dd");
                string gmtTimeString = gmtDateTimeOffset.ToString("HH:mm tt");
                string imageUrl = $"https://royalmotors.azurewebsites.net/image/{request.CarName.Replace(" ", "_")}_2";
                Email.Email.sendEmail(request.AccountEmail, "Test Drive Appointment Reserved", HTMLContent.HTMLContent.TestdriveCreatedEmail(account.firstname, gmtDateString, gmtTimeString, car.name, imageUrl));
            }
            catch { }
            return CreatedAtAction(nameof(AddTestDrive), testDrive);
        }
        catch (Exception e)
        {
            if(e is ProfileNotFoundException)
            {
                return NotFound($"User with email {request.AccountEmail} not found");
            }
            else if(e is CarNotFoundException)
            {
                return NotFound($"Car {request.CarName} not found");
            }
            else if(e is TestDriveConflictException)
            {
                return Conflict(e.Message);
            }
            throw;
        }
    }

    [HttpDelete("{Id}"), Authorize]
    public async Task<IActionResult> DeleteTestDrive(int Id)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != await testdriveInterface.GetAccount(Id) && emailClaim != "royalmotorslb@gmail.com")
            {
                //return Unauthorized("You are not authorized!");
            }
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

    [HttpGet("{Id}"), Authorize]
    public async Task<ActionResult<TestDrive>> GetTestDriveByTestDriveId(int Id)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != await testdriveInterface.GetAccount(Id) && emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
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

    [HttpGet("car/{Car_Name}"),Authorize]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByCarName(string Car_Name)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByCarName(Car_Name);
            return Ok(test_drives);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"No test drives found for car {Car_Name}.");
            }
            throw;
        }

    }

    [HttpGet("account/{Account_Email}"), Authorize]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDriveByAccountEmail(string Account_Email)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != Account_Email && emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
        if (!Account_Email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDriveByAccountEmail(Account_Email);
            return Ok(test_drives);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound($"No test drives found for user {Account_Email}.");
            }
            throw;
        }
    }

    [HttpGet, Authorize]
    public async Task<ActionResult<List<TestDrive>>> GetAllTestDrives()
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
        try
        {
            var test_drives = await testdriveInterface.GetAllTestDrives();
            return Ok(test_drives);
        }
        catch (Exception e)
        {
            if (e is TestDriveNotFoundException)
            {
                return NotFound("No test drives have been scheduled.");
            }
            throw;
        }
    }

    [HttpGet("slots/{Car_Name}")]
    public async Task<ActionResult<List<long>>> GetAllTestDriveTimesByCarName(string Car_Name)
    {
        try
        {
            var slots = await testdriveInterface.GetAllAvailableSlots(Car_Name);
            return Ok(slots);
        }
        catch (Exception e)
        {
            if (e is CarNotFoundException)
            {
                return NotFound($"Car with name {Car_Name} was not found.");
            }
            throw;
        }

    }
}

