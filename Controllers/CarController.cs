using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("car")]
public class CarController : ControllerBase
{
    private readonly CarInterface carInterface;

    public CarController(CarInterface carInterface)
    {
        this.carInterface = carInterface;
    }

    [HttpPost]
    public async Task<ActionResult<Car>> AddCar(Car car)
    {
        try
        {
            await carInterface.AddCar(car);
            return CreatedAtAction(nameof(AddCar), new { name = car.name }, car);
        }
        catch (DuplicateCarException e)
        {
            return Conflict("Car already added. Try to add another car.");
        }
    }

    [HttpGet("{name}")]
    public async Task<ActionResult<Car>> GetCar(string name)
    {
      
        //not really needed since this won't be called unless a user is signed in and opened his account
        try
        {
            var car = await carInterface.GetCar(name);
            return Ok(car);
        }
        catch (CarNotFoundException e)
        {
            return NotFound($"Car with name {name} not found.");
        }
    }

    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteCar(string name)
    {
   
        try
        {
            await carInterface.DeleteCar(name);
            return Ok("Car successfully deleted");
        }
        
        catch (CarNotFoundException e)
        {

            return NotFound($"Car with name {name} not found.");
        }

    }

    [HttpPut]
    public async Task<ActionResult<Car>> Edit(Car car)
    {
        try
        {
            Car new_car = new Car(car.name, car.make, car.model, car.year, car.color, car.used, car.price, car.description,
            car.mileage, car.image_id_list, car.video_id);

            //await is related to async, wait it to sync.  
            await carInterface.EditCar(new_car);
            return CreatedAtAction(nameof(Edit), new { name = new_car.name }, new_car);
        }
        catch (CarNotFoundException e)
        {
            return NotFound($"Car with name {car.name} not found.");
        }
    }
}