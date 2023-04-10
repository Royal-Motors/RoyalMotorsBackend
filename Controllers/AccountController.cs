using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Email;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using Microsoft.AspNetCore.Rewrite;
using System;
using CarWebsiteBackend.HTMLContent;

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

    private ContentResult verifySuccess()
    {
        return new ContentResult { Content = HTMLContent.HTMLContent.verifySuccessWebsite(), ContentType = "text/html" };
    }

    private ContentResult verifyFail()
    {
        return new ContentResult { Content = HTMLContent.HTMLContent.verifyFailWebsite(), ContentType = "text/html" };
    }

    private ContentResult alreadyVerified()
    {
        return new ContentResult { Content = HTMLContent.HTMLContent.alreadyVerifiedWebsite(), ContentType = "text/html" };
    }

    [HttpPost("sign_up")]
    public async Task<ActionResult<Account>> SignUp(CreateAccount create_account)
    {
        if(!IsValidEmail(create_account.email))
        {
            return BadRequest("Invalid email format.");
        }
        try
        {
            string code = Guid.NewGuid().ToString("N");
            var account = new Account(create_account.email, create_account.password, create_account.firstname, create_account.lastname, false, code);
            await accountInterface.AddAccount(account);
            string link = $"https://royalmotors.azurewebsites.net/account/verify/{create_account.email}/{code}";
            Email.Email.sendEmail(account.email, "Verification Code", HTMLContent.HTMLContent.emailBody(link));
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

    [HttpPost("verify/{email}/{code}")]
    public async Task<ActionResult<Account>> verify(string email, string code)
    {
        try
        {
            var account = await accountInterface.GetAccount(email);
            if (account.verified) return alreadyVerified();
            if (account.verificationCode != null && account.verificationCode == code)
            {
                await accountInterface.VerifyAccount(email);
                return verifySuccess();
            }
            else return verifyFail();
        }
        catch (Exception e)
        {
            return verifyFail();
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
            if (!account.verified)
            {
                return Unauthorized("Unverified email address");
            }
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
