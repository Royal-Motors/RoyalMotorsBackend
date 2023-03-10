using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWebsiteBackend.DTOs;
namespace CarWebsiteBackend.Tests.DTO_Tests;

public class EmailTests
{
    [Fact]
    public void Email_ValidAddress()
    {
        string validAddress = "foo.bar@example.com";
        Email email = new(validAddress);
        Assert.Equal(validAddress, email.address);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email-address")]
    public void Email_InvalidAddress_NullOrWhiteSpace(string address)
    {
        Assert.Throws<ArgumentException>(() => new Email(address));
    }
}
