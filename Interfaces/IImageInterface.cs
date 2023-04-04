using CarWebsiteBackend.DTOs;
using Microsoft.AspNetCore.Mvc;
namespace CarWebsiteBackend.Interfaces;

public interface IImageInterface
{
    Task<Image?> DownloadImage(string? id);

    Task DeleteImage(string? id);

    Task UploadImage(string name, IFormFile file);

}
