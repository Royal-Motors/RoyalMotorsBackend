using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Exceptions;
namespace CarWebsiteBackend.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    private readonly IAccountInterface accountInterface;

    public AccountController(IAccountInterface accountInterface)
    {
        this.accountInterface = accountInterface;
    }


    [HttpPost("sign_up")]
    public async Task<ActionResult<Account>> SignUp(Account account)
    {
        if(!IsValidEmail(account.email))
        {
            return BadRequest("Invalid email format.");
        }

        try
        {
            await accountInterface.AddAccount(account);
            return CreatedAtAction(nameof(SignUp), new { email = account.email }, account);
        }
        catch(ProfileAlreadyExistsException e){
            return Conflict("Email already taken. Sign up with another email, or sign in.");
        }

    }


    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            // use regex to validate email format
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}