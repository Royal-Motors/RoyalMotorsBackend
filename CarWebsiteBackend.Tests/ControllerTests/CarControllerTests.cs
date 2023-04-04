using System;
using System.Collections.Generic;
using System.Linq;
using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.CarExceptions;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using CarWebsiteBackend.Exceptions;

namespace CarWebsiteBackend.Tests.ControllerTests;

public class CarControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<CarInterface> carStoreMock = new();
    private readonly HttpClient httpClient;

    private readonly Car testCar;


    public CarControllerTests(WebApplicationFactory<Program> factory)
    {
        httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(carStoreMock.Object);
            });
        }).CreateClient();

        testCar = new Car("name", "make", "model", 2020, "color", true, 20000, "description", 100000, "image_id_list", "video_id");
    }

    [Fact]
    public async Task AddCar_Valid()
    {
        // Arrange
        carStoreMock.Setup(m => m.AddCar(testCar)).Returns(Task.CompletedTask);


        // Act
        var response = await httpClient.PostAsync("/car",
            new StringContent(JsonConvert.SerializeObject(testCar), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var uploadedCar = JsonConvert.DeserializeObject<Car>(json);
        Assert.Equal(testCar, uploadedCar);

        carStoreMock.Verify(m => m.AddCar(testCar), Times.Once);
    }

    [Fact]
    public async Task AddCar_DuplicateCar()
    {
        // Arrange
        carStoreMock.Setup(m => m.AddCar(testCar)).Throws(new DuplicateCarException());

        // Act
        var response = await httpClient.PostAsync("/car",
            new StringContent(JsonConvert.SerializeObject(testCar), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        carStoreMock.Verify(m => m.AddCar(testCar), Times.Once);
    }

}
