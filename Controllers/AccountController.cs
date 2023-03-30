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
        catch(Exception e){
            if (e is ProfileAlreadyExistsException)
            {
                return Conflict("Email already taken. Sign up with another email, or sign in.");
            }
            throw;
        }

    }

// needs cleaning up and add login API

    [HttpGet("{email}")]
    public async Task<ActionResult<Account>> GetAccount(string email)
    {
        if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
        //not really needed since this won't be called unless a user is signed in and opened his account
        try
        {
            var account = await accountInterface.GetAccount(email);
            return Ok(account);
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Account with email {email} not found.");
            }
            throw;
        }
    }

    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> DeleteAccount(string email)
    {
        if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
        try
        {
            await accountInterface.DeleteAccount(email);
            return Ok("Account successfully deleted");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {

                return NotFound($"Account with email {email} not found.");
            }
            throw;
        }

    }

    [HttpPut("edit/{email}")]
    public async Task<ActionResult<Account>> Edit(EditedAcc editedAcc, string email)
    {
        if (!IsValidEmail(email))
        {
            return BadRequest("Invalid email format");
        }
        try
        {
            Account new_acc = new Account(email, editedAcc.password, editedAcc.firstname, editedAcc.lastname);
            //await is related to async, wait it to sync.  
            await accountInterface.ReplaceAccount(new_acc);
            return CreatedAtAction(nameof(Edit), new { email = new_acc.email }, new_acc);
        }
        catch (Exception e)
        { 
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Account with email {email} not found.");
            }
            throw;
        }
    }
    [HttpGet("sign_in")]
    public async Task<ActionResult<Account>> SignIn(string email, string password)
    {
        if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            return BadRequest("Password cannot be empty");
        }
        try{
            var account = await accountInterface.GetAccount(email);
            if (account.password == password){
                return Ok(account);
            }
            return Unauthorized("Incorrect password.");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"Account with email {email} not found.");
            }
            throw;
        }
    }
}
