﻿using System.Net;
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
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPost, Authorize]
    public async Task<ActionResult<Car>> AddCar(CreateCar create_car)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to edit this account.");
        }
        try
        {
            var car = new Car(create_car.name, create_car.make, create_car.model, create_car.year, create_car.color, create_car.used, create_car.price, create_car.description, create_car.mileage, create_car.horsepower, create_car.fuelconsumption, create_car.fueltankcapacity, create_car.transmissiontype, create_car.image_id_list, create_car.video_id);
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
    public async Task<ActionResult<Car>> GetCar(string name)
    {
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

    [HttpDelete("{name}"), Authorize]
    public async Task<IActionResult> DeleteCar(string name)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to edit this account.");
        }
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

    [HttpPut, Authorize]
    public async Task<ActionResult<Car>> Edit(Car car)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to edit this account.");
        }
        try
        {
            Car new_car = new Car(car.name, car.make, car.model, car.year, car.color, car.used, car.price, car.description,
            car.mileage, car.horsepower, car.fuelconsumption, car.fueltankcapacity, car.transmissiontype, car.image_id_list, car.video_id);

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