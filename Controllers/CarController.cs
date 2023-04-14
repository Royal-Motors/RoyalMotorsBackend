using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Exceptions;
using Microsoft.Extensions.Options;
using System.Drawing;
using Microsoft.AspNetCore.Cors;
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
    [EnableCors("AllowAnyOrigin")]
    public async Task<ActionResult<Car>> AddCar(CreateCar create_car)
    {
        try
        {
            var car = new Car(create_car.name, create_car.make, create_car.model, create_car.year, create_car.color, create_car.used, create_car.price, create_car.description, create_car.mileage, create_car.image_id_list, create_car.video_id);
            await carInterface.AddCar(car);
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
    [EnableCors("AllowAnyOrigin")]
    public async Task<ActionResult<Car>> GetCar(string name)
    {
      
        //not really needed since this won't be called unless a user is signed in and opened his account
        try
        {
            var car = await carInterface.GetCar(name);
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
    [EnableCors("AllowAnyOrigin")]
    public async Task<ActionResult<List<Car>>> GetAllCars()
    {
        try
        {
            var cars = await carInterface.GetAllCars();
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
    [EnableCors("AllowAnyOrigin")]
    public async Task<IActionResult> DeleteCar(string name)
    {
   
        try
        {
            await carInterface.DeleteCar(name);
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

    [HttpPut]
    [EnableCors("AllowAnyOrigin")]
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
        catch (Exception e)
        {
            if(e is CarNotFoundException)
            {
                return NotFound($"Car with name {car.name} not found.");
            }
            throw;
        }
    }
}