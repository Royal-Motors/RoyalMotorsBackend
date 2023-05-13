using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using CarWebsiteBackend.Exceptions;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly IImageInterface imageInterface;
    private readonly CarInterface carInterface;

    public ImageController(IImageInterface imageInterface, CarInterface carInterface)
    {
        this.imageInterface = imageInterface;
        this.carInterface = carInterface;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IFormFile>> GetImage(string? id)
    {
        try
        {
            var image = await imageInterface.DownloadImage(id);
            var fileContent = new byte[image.Content.Length];
            await image.Content.ReadAsync(fileContent, 0, fileContent.Length);
            return File(fileContent, image.ContentType);
        }
        catch (Exception e)
        {
            if (e is ImageNotFoundException)
            {
                return NotFound($"Image with id {id} not found");
            }
            throw;
        }
    }

    [HttpDelete, Authorize]
    public async Task<ActionResult> DeleteImage(string carName, int order)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
        try
        {
            var car = await carInterface.GetCar(carName);
            string image_id_list = car.image_id_list;
            string[] imageArray = image_id_list.Split(',');
            string id = imageArray[order - 1];
            imageArray[order - 1] = "";
            string edited_image_id_list = string.Join(",", imageArray);
            var editedCar = new Car(car.name, car.make, car.model, car.year, car.color, car.used, car.price, car.description, car.mileage, car.horsepower, car.fuelconsumption, car.fueltankcapacity, car.transmissiontype, edited_image_id_list, car.video_id);
            await carInterface.EditCar(editedCar);
            await imageInterface.DeleteImage(id);
            return Ok($"Image with id {id} deleted");
        }
        catch (Exception e)
        {
            if (e is ImageNotFoundException)
            {
                return NotFound($"Image not found");
            }
            throw;
        }
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> PostImage(IFormFile File, string carName, int order)
    {
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if(emailClaim != "royalmotorslb@gmail.com")
            {
                return Unauthorized("You are not authorized to edit this account.");
            }
        try
        {
            var type = File.ContentType;
            if (type != "image/jpeg" && type != "image/png")
            {
                return BadRequest("Content type not supported, upload a 'jpeg' or 'png'");
            }
            var car = await carInterface.GetCar(carName);
            string image_id_list = car.image_id_list;
            string[] imageArray = image_id_list.Split(',');
            string carNameNoSpace = carName.Replace(" ", "_");
            string carNameEdited = $"{carNameNoSpace}_{order}";
            imageArray[order - 1] = carNameEdited;
            string edited_image_id_list = string.Join(",", imageArray);

            var editedCar = new Car(car.name, car.make, car.model, car.year, car.color, car.used, car.price, car.description,
                car.mileage, car.horsepower, car.fuelconsumption, car.fueltankcapacity, car.transmissiontype,
                edited_image_id_list, car.video_id);

            await imageInterface.UploadImage(carNameEdited, File);
            await carInterface.EditCar(editedCar);
            return Ok("Image successfully uploaded.");
        }
        catch (Exception e)
        {
            if (e is CarNotFoundException)
            {
                return NotFound("car not found");
            }
            throw;
        }
    }
}