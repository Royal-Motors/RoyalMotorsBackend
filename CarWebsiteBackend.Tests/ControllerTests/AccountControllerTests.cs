using CarWebsiteBackend.DTOs;
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

        testAccount = new Account(new Email("foo.bar@example.com"), "password", "Foo", "Bar");
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
        accountStoreMock.Setup(m => m.AddAccount(testAccount)).Throws(new Exception("Email taken"));

        // Act
        var response = await httpClient.PostAsync("/account/sign_up",
            new StringContent(JsonConvert.SerializeObject(testAccount), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(m => m.AddAccount(testAccount), Times.Once);
    }

    [Theory]
    [InlineData("", "Foo", "Bar")]
    [InlineData(" ", "Foo", "Bar")]
    [InlineData(null, "Foo", "Bar")]
    [InlineData("pass", "", "Bar")]
    [InlineData("pass", " ", "Bar")]
    [InlineData("pass", null, "Bar")]
    [InlineData("pass", "Foo", "")]
    [InlineData("pass", "Foo", " ")]
    [InlineData("pass", "Foo", null)]
    public async Task SignUp_InvalidArguments(string password, string firstname, string lastname)
    {
        Account invalidAccount = new(testAccount.email, password,firstname, lastname);
        var response = await httpClient.PostAsync("/account/sign_up",
            new StringContent(JsonConvert.SerializeObject(invalidAccount), Encoding.Default, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.AddAccount(invalidAccount), Times.Never);
    }
}