using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions;
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

namespace CarWebsiteBackend.Tests.ControllerTests;

public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Mock<IAccountInterface> accountStoreMock = new();
    private readonly HttpClient httpClient;

    private readonly Account testAccount;

    public AccountControllerTests(WebApplicationFactory<Program> factory)
    {
        httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton(accountStoreMock.Object);
            });
        }).CreateClient();

        testAccount = new Account("foo.bar@example.com", "password", "Foo", "Bar");
    }

    [Fact]
    public async Task SignUp_Valid()
    {
        // Arrange
        accountStoreMock.Setup(m => m.AddAccount(testAccount)).Returns(Task.CompletedTask);


        // Act
        var response = await httpClient.PostAsync("/account/sign_up",
            new StringContent(JsonConvert.SerializeObject(testAccount), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var uploadedAccount = JsonConvert.DeserializeObject<Account>(json);
        Assert.Equal(testAccount, uploadedAccount);

        accountStoreMock.Verify(m => m.AddAccount(testAccount), Times.Once);
    }

    [Fact]
    public async Task SignUp_EmailTaken()
    {
        // Arrange
        accountStoreMock.Setup(m => m.AddAccount(testAccount)).Throws(new ProfileAlreadyExistsException());

        // Act
        var response = await httpClient.PostAsync("/account/sign_up",
            new StringContent(JsonConvert.SerializeObject(testAccount), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        accountStoreMock.Verify(m => m.AddAccount(testAccount), Times.Once);
    }

    [Theory]
    [InlineData("", "pass", "Foo", "Bar")]
    [InlineData(" ", "pass", "Foo", "Bar")]
    [InlineData(null, "pass", "Foo", "Bar")]
    [InlineData("Invalid-email", "pass", "Foo", "Bar")]
    [InlineData("foo.bar@example.com", "", "Foo", "Bar")]
    [InlineData("foo.bar@example.com", " ", "Foo", "Bar")]
    [InlineData("foo.bar@example.com", null, "Foo", "Bar")]
    [InlineData("foo.bar@example.com", "pass", "", "Bar")]
    [InlineData("foo.bar@example.com", "pass", " ", "Bar")]
    [InlineData("foo.bar@example.com", "pass", null, "Bar")]
    [InlineData("foo.bar@example.com", "pass", "Foo", "")]
    [InlineData("foo.bar@example.com", "pass", "Foo", " ")]
    [InlineData("foo.bar@example.com", "pass", "Foo", null)]
    public async Task SignUp_InvalidArguments(string email, string password, string firstname, string lastname)
    {
        Account invalidAccount = new(email, password,firstname, lastname);
        var response = await httpClient.PostAsync("/account/sign_up",
            new StringContent(JsonConvert.SerializeObject(invalidAccount), Encoding.Default, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.AddAccount(invalidAccount), Times.Never);
    }


}