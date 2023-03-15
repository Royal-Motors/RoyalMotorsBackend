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

// needs cleaning up and add login API

    [HttpGet("get/{email}")]
        public async Task<ActionResult<Account>> GetAccounts(string email)
        {
            if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
            //not really needed since this won't be called unless a user is signed in and opened his account
            try{
                var account = await accountInterface.GetAccount(email);
                return account;
            }
            catch(ProfileNotFoundException e){
                return Conflict("Email does not exist!");
            }
        }

    [HttpDelete("delete/{email}")]
        public async Task<IActionResult> DeleteAccountsItem(string email)
        {
            if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
            try{
                await accountInterface.DeleteAccount(email);
                return NoContent();
            }
            //also not really needed since an account will only be deleted when the user presses on delete account in his own profile 
            catch(ProfileNotFoundException e){

                return Conflict("Email does not exist!");
            }

        }
}