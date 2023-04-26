using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions;

namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase
{
    private readonly IImageInterface imageInterface;

    public ImageController(IImageInterface imageInterface)
    {
        this.imageInterface = imageInterface;
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

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteImage(string? id)
    {
        try
        {
            await imageInterface.DeleteImage(id);
            return Ok($"Image with id {id} deleted");
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


    [HttpPost]
    public async Task<IActionResult> PostImage(IFormFile File)
    {
        var type = File.ContentType;
        if (type != "image/jpeg" && type != "image/png")
        {
            return BadRequest("Content type not supported, upload a 'jpeg' or 'png'");
        }
        await imageInterface.UploadImage(Path.GetFileNameWithoutExtension(File.FileName), File);
        return Ok("Image successfully uploaded.");
    }
}
