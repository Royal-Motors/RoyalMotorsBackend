using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWebsiteBackend.DTOs;

namespace CarWebsiteBackend.Tests.DTO_Tests;

public class AccountTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("invalid-email-address")]
    public void InvalidAccount_BadEmail(string address)
    {
        Assert.Throws<ArgumentException>(() =>
            new Account(new Email(address), "passcode", "Foo", "Bar")
        );
    }
    // To add in Controller Tests
    //[Theory]
    //[InlineData(null, "Foo", "Bar")]
    //[InlineData("", "Foo", "Bar")]
    //[InlineData(" ", "Foo", "Bar")]
    //[InlineData("passcode", null, "Bar")]
    //[InlineData("passcode", "", "Bar")]
    //[InlineData("passcode", "   ", "Bar")]
    //[InlineData("passcode", "Foo", "")]
    //[InlineData("passcode", "Foo", null)]
    //[InlineData("passcode", "Foo", " ")]
    //public void InvalidAccount_BadParams(string password, string firstname, string lastname)
    //{
    //    Assert.Throws<ArgumentException>(() =>
    //        new Account(new Email("foo.bar@example.com"), password, firstname, lastname)
    //    );;
    //}
}

