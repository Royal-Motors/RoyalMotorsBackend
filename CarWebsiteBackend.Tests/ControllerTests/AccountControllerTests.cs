using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
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
    private readonly EditedAcc testEdit;

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
        testEdit = new EditedAcc("password2", "Foo2", "Bar2");
    }


    /*
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
    */

    /*
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

    */

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

    [Fact]
    public async Task DeleteAccount_Valid()
    {
        accountStoreMock.Setup(m => m.DeleteAccount(testAccount.email)).Returns(Task.CompletedTask);

        var response = await httpClient.DeleteAsync($"/account/delete/{testAccount.email}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        accountStoreMock.Verify(mock => mock.DeleteAccount(testAccount.email), Times.Once);
    }

    [Fact]
    public async Task DeleteAccount_NotFound()
    {
        accountStoreMock.Setup(m => m.DeleteAccount(testAccount.email)).Throws(new ProfileNotFoundException());

        var response = await httpClient.DeleteAsync($"/account/delete/{testAccount.email}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        accountStoreMock.Verify(mock => mock.DeleteAccount(testAccount.email), Times.Once);
    }

    [Fact]
    public async Task DeleteAccount_InvalidEmail()
    {
        // no need to check for null, empty, or white space -- guaranteed from URL
        string badEmail = "invalid-email";
        var response = await httpClient.DeleteAsync($"/account/delete/{badEmail}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.DeleteAccount(badEmail), Times.Never);
    }

    [Fact]
    public async Task GetAccount_Valid()
    {
        accountStoreMock.Setup(m => m.GetAccount(testAccount.email)).ReturnsAsync(testAccount);

        var response = await httpClient.GetAsync($"/account/{testAccount.email}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        accountStoreMock.Verify(mock => mock.GetAccount(testAccount.email), Times.Once);
    }

    [Fact]
    public async Task GetAccount_NotFound()
    {
        accountStoreMock.Setup(m => m.GetAccount(testAccount.email)).Throws(new ProfileNotFoundException());

        var response = await httpClient.GetAsync($"/account/{testAccount.email}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        accountStoreMock.Verify(mock => mock.GetAccount(testAccount.email), Times.Once);
    }

    [Fact]
    public async Task GetAccount_InvalidEmail()
    {
        // no need to check for null, empty, or white space -- guaranteed from URL
        string badEmail = "invalid-email";
        var response = await httpClient.GetAsync($"/account/{badEmail}");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.GetAccount(badEmail), Times.Never);
    }

    /*
    [Fact]
    public async Task EditAccount_ValidEmail()
    {
        // Arrange
        Account checkAccount = new(testAccount.email, testEdit.password, testEdit.firstname, testEdit.lastname);
        accountStoreMock.Setup(m => m.ReplaceAccount(checkAccount)).Returns(Task.CompletedTask);
        // Act
        var response = await httpClient.PutAsync($"/account/edit/{checkAccount.email}",
            new StringContent(JsonConvert.SerializeObject(testEdit), Encoding.Default, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        var uploadedAccount = JsonConvert.DeserializeObject<Account>(json);

        Assert.Equal(checkAccount, uploadedAccount);

        accountStoreMock.Verify(m => m.ReplaceAccount(checkAccount), Times.Once);
    }
    */

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
    public async Task EditAccount_InvalidArgs(string password, string firstname, string lastname)
    {
        Account invalidAccount = new(testAccount.email, password, firstname, lastname);
        var response = await httpClient.PutAsync($"/account/edit/{invalidAccount.email}",
            new StringContent(JsonConvert.SerializeObject(invalidAccount), Encoding.Default, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.ReplaceAccount(invalidAccount), Times.Never);
    }

    [Fact]
    public async Task EditAccount_InvalidEmail()
    {
        string badEmail = "invalid-email";
        Account invalidAccount = new(badEmail, testEdit.password, testEdit.firstname, testEdit.lastname);

        var response = await httpClient.PutAsync($"/account/edit/{badEmail}",
             new StringContent(JsonConvert.SerializeObject(invalidAccount), Encoding.Default, "application/json"));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        accountStoreMock.Verify(mock => mock.AddAccount(invalidAccount), Times.Never);
    }

    [Fact]
    public async Task EditAccount_NotFound()
    {
        accountStoreMock.Setup(m => m.ReplaceAccount(testAccount)).Throws(new ProfileNotFoundException());
        
        var response = await httpClient.PutAsync($"/account/edit/{testAccount.email}",
             new StringContent(JsonConvert.SerializeObject(testAccount), Encoding.Default, "application/json"));

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        accountStoreMock.Verify(mock => mock.ReplaceAccount(testAccount), Times.Once);
    }
}