using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions;
using CarWebsiteBackend.Interfaces;

namespace CarWebsiteBackend.Storage;

public class ImageBlobStorage : IImageInterface
{
    private readonly BlobServiceClient blobServiceClient;

    public ImageBlobStorage(BlobServiceClient _blobServiceClient)
    {
        blobServiceClient = _blobServiceClient;
    }

    private BlobContainerClient Container => blobServiceClient.GetBlobContainerClient("carimages");

    public async Task<Image?> DownloadImage(string id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException("Id cannot be null or empty");
        }
        try
        {
            string type;
            BlobClient blobClient = Container.GetBlobClient(id + ".png");
            type = "png";

            if (!await blobClient.ExistsAsync()) // if {id}.png doesn't exist
            {
                blobClient = Container.GetBlobClient(id + ".jpeg");
                type = "jpeg";
                if (!await blobClient.ExistsAsync()) // if {id}.jpeg doesn't exist
                {
                    throw new ImageNotFoundException();
                }
            }

            BlobDownloadResult content = await blobClient.DownloadContentAsync();
            var stream = content.Content.ToStream();
            return new Image(stream, "image/" + type);
        }
        catch
        {
            throw;
        }
    }


    public async Task UploadImage(string name, IFormFile file)
    {
        string type = Path.GetExtension(file.FileName).ToLower();
        if (type == ".jpg")
        {
            type = ".jpeg";
        }
        var blobName = $"{name}{type}"; //file type to include in Image during download
        var blobClient = Container.GetBlobClient(blobName);
        await blobClient.UploadAsync(file.OpenReadStream(), true);
        return;
    }

    public async Task DeleteImage(string? id)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentNullException();
        }
        try
        {
            BlobClient blobClient = Container.GetBlobClient(id + ".png");
            if (!await blobClient.ExistsAsync()) // if {id}.png doesn't exist
            {
                blobClient = Container.GetBlobClient(id + ".jpeg");
                if (!await blobClient.ExistsAsync()) // if {id}.jpeg doesn't exist
                {
                    throw new ImageNotFoundException();
                }

            }
            await blobClient.DeleteAsync();
            return;
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteAllImages(string[] imageList)
    {
        try
        {
            foreach (var image in imageList)
            {
                BlobClient blobClient = Container.GetBlobClient(image + ".png");
                if (!await blobClient.ExistsAsync()) // if {id}.png doesn't exist
                {
                    blobClient = Container.GetBlobClient(image + ".jpeg");
                    if (!await blobClient.ExistsAsync()) // if {id}.jpeg doesn't exist
                    {
                        continue;
                    }

                }
                await blobClient.DeleteAsync();
            }
            return;
        }
        catch
        {
            throw;
        }
    }
}
