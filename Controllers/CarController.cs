using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions.CarExceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Azure.Core;
using CarWebsiteBackend.Exceptions.ProfileExceptions;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("car")]
public class CarController : ControllerBase
{
    private readonly CarInterface carStore;
    private readonly ITestDriveInterface testdriveStore;
    private readonly IAccountInterface accountStore;
    private readonly IImageInterface imageStore;
    private readonly ISaleStore saleStore;


    public CarController(CarInterface carInterface, ITestDriveInterface testdriveInterface,
        IAccountInterface accountStore, IImageInterface imageStore, ISaleStore saleStore)
    {
        this.carStore = carInterface;
        this.testdriveStore = testdriveInterface;
        this.accountStore = accountStore;
        this.imageStore = imageStore;
        this.saleStore = saleStore;
    }

    private async Task RemoveCarAndImages(string carname)
    {
        var imageIDs = await GetImageIDs(carname);
        await Task.WhenAll(carStore.DeleteCar(carname),
                            imageStore.DeleteAllImages(imageIDs)
        );
    }

    [HttpPost, Authorize]
    public async Task<ActionResult<Car>> AddCar(CreateCar create_car)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
        try
        {
            var car = new Car(create_car.name, create_car.make, create_car.model, create_car.year, create_car.color, create_car.used, create_car.price, create_car.description, create_car.mileage, create_car.horsepower, create_car.fuelconsumption, create_car.fueltankcapacity, create_car.transmissiontype, create_car.image_id_list, create_car.video_id);
            await carStore.AddCar(car);
            return CreatedAtAction(nameof(AddCar), new { name = car.name }, car);
        }
        catch (Exception e)
        {
            if(e is DuplicateCarException)
            {
                return Conflict("Car already added. Try to add another car.");
            }
            throw;
        }
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Car>> GetCar(string name)
    {
        try
        {
            var car = await carStore.GetCar(name);
            return Ok(car);
        }
        catch (Exception e)
        {
            if(e is CarNotFoundException)
            {
                return NotFound($"Car with name {name} not found.");
            }
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<Car>>> GetAllCars()
    {
        try
        {
            var cars = await carStore.GetAllCars();
            return Ok(cars);
        }
        catch (Exception e)
        {
            if(e is CarNotFoundException)
            {
                return NotFound($"No cars found.");
            }
            throw;
        }

    }

    [HttpDelete("{name}"), Authorize]
    public async Task<IActionResult> DeleteCar(string name)
    {   
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized!");
        }
        try
        {
            await sendEmailsDeletion(name);
            await RemoveCarAndImages(name);
            return Ok("Car successfully deleted");
        }
        
        catch (Exception e)
        {
            if(e is CarNotFoundException)
            {
                return NotFound($"Car with name {name} not found.");
            }
            throw;
        }
    }

    [HttpDelete("sell"),Authorize]
    public async Task<IActionResult> SellCar(Sale sale)
    {
        string name = sale.CarName;
        string email = sale.Email;

        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized!");
        }
        try
        {
            await accountStore.GetAccount(email);
            await carStore.GetCar(name);
            await saleStore.AddSale(sale);
            await sendEmailsDeletion(name);
            await RemoveCarAndImages(name);
            return Ok("Car successfully counted Sold");
        }

        catch (Exception e)
        {
            if (e is CarNotFoundException)
            {
                return NotFound($"Car with name {name} not found.");
            }
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Account with email {email} not found.");
            }
            throw;
        }
    }

    [HttpPut("edit/{name}"),Authorize]
    public async Task<ActionResult<Car>> Edit(EditedCar editedCar, string name)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized!");
            }
        try
        {
            Car new_car = new Car(name, editedCar.make, editedCar.model, editedCar.year, editedCar.color, editedCar.used, editedCar.price, editedCar.description,
            editedCar.mileage, editedCar.horsepower, editedCar.fuelconsumption, editedCar.fueltankcapacity, editedCar.transmissiontype, editedCar.image_id_list, editedCar.video_id);

            //await is related to async, wait it to sync.  
            await carStore.EditCar(new_car);
            return CreatedAtAction(nameof(Edit), new { name = new_car.name }, new_car);
        }
        catch (Exception e)
        {
            if(e is CarNotFoundException)
            {
                return NotFound($"Car with name {name} not found.");
            }
            throw;
        }
    }

    private async Task<string[]> GetImageIDs(string name)
    {
        var car =  await carStore.GetCar(name);
        return car.image_id_list.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

    }

    private async Task sendEmailsDeletion(string name)
    {
        try
        {
            List<TestDrive> testDrivesList = await testdriveStore.GetAllTestDriveByCarName(name);
            var image = await imageStore.DownloadImage($"{name.Replace(" ", "_")}_2");
            foreach (TestDrive testDrive in testDrivesList)
            {
                Email.Email.sendEmail(testDrive.Account.email, name + " Has Been Sold", HTMLContent.HTMLContent.CarSoldEmail(testDrive.Account.firstname, name), image.Content);
            }
        }
        catch { }

    }
}