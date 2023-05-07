using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Collections.Generic;
using System.Collections;
using CarWebsiteBackend.Storage;



namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("car")]
public class CarController : ControllerBase
{
    private readonly CarInterface carStore;
    private readonly ITestDriveInterface testdriveStore;
    private readonly IAccountInterface accountStore;
    private readonly IImageInterface imageStore;


    public CarController(CarInterface carInterface, ITestDriveInterface testdriveInterface,
        IAccountInterface accountStore, IImageInterface imageStore)
    {
        this.carStore = carInterface;
        this.testdriveStore = testdriveInterface;
        this.accountStore = accountStore;
        this.imageStore = imageStore;
    }

    [HttpPost]
    public async Task<ActionResult<Car>> AddCar(CreateCar create_car)
    {
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

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteCar(string name)
    {   
        try
        {
            var imageIDs = await GetImageIDs(name);
            await Task.WhenAll(carStore.DeleteCar(name),
                                imageStore.DeleteAllImages(imageIDs)
            );
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

    [HttpDelete("sell/{name}")]
    public async Task<IActionResult> SellCar(string name)
    {
        try
        {
            List<TestDrive> testDrivesList = await testdriveStore.GetAllTestDriveByCarName(name);
            var imageIDs = await GetImageIDs(name);
            await Task.WhenAll(carStore.SellCar(name),
                                imageStore.DeleteAllImages(imageIDs)
            );
            foreach (TestDrive testDrive in testDrivesList)
            {
                Email.Email.sendEmail(testDrive.Account.email, name + " Car Has Been Sold", HTMLContent.HTMLContent.CarSoldEmail(testDrive.Account.firstname + " " + testDrive.Account.lastname, name));
            }
            return Ok("Car successfully counted Sold");
        }

        catch (Exception e)
        {
            if (e is CarNotFoundException)
            {
                return NotFound($"Car with name {name} not found.");
            }
            throw;
        }
    }

    [HttpPut("edit/{name}")]
    public async Task<ActionResult<Car>> Edit(EditedCar editedCar, string name)
    {
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
}