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

    [HttpGet("get/{address}")]
        public async Task<ActionResult<Account>> GetAccounts(string address)
        {
            Email email = new(address);
            var account = await accountInterface.GetAccount(email);
            if (account == null)
            {
                return NotFound();
            }
                return account;
        }

    [HttpDelete("delete/{address}")]
        public async Task<IActionResult> DeleteAccountsItem(string address)
        {
            Email email = new(address);
            var account = await accountInterface.GetAccount(email);
            if (email == null)
            {
                return NotFound();
            }
            try{
                await accountInterface.DeleteAccount(email);
                return NoContent();
            }
            catch{
                return BadRequest("Email doesn't exist.");
            }

        }
}