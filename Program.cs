using Azure.Storage.Blobs;
using CarWebsiteBackend.Configuration;
using CarWebsiteBackend.Controllers;
using CarWebsiteBackend.Data;
using CarWebsiteBackend.Interfaces;
using CarWebsiteBackend.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));
builder.Services.AddScoped<IAccountInterface, AccountStorage>();
builder.Services.AddScoped<CarInterface, CarStorage>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IAccountInterface, InMemoryStore>();
builder.Services.AddSingleton<IImageInterface, ImageBlobStorage>();
builder.Services.AddSingleton(sp =>
{
    return new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=blobstorage98754;AccountKey=IMl/5Xr9F4+WSlaf3ajHLd6k34eIf9Owson7ds3FEJd1yL+VTiAP7Rka8VXHBTwGEXfdpXDnDhJT+AStwwzmPw==;EndpointSuffix=core.windows.net");
});





var app = builder.Build();

// Configure the HTTP request pipeline. 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }