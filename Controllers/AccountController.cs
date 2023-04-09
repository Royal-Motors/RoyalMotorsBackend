using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Email;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using Microsoft.AspNetCore.Rewrite;
using System;

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

            string link = $"https://localhost:7284/account/verify/{create_account.email}/{code}";
            string emailBody = @"
            <html>
                <head>
                    <title>Royal Motors - Email Verification</title>
                    <style>
                        body {
                            margin: 0;
                            padding: 0;
                            font-family: Arial, sans-serif;
                            font-size: 16px;
                            color: #444444;
                            }
                        .header {
                            background-color: #0d47a1;
                            color: #ffffff;
                            padding: 20px;
                            }
                        .header h1 {
                            margin: 0;
                            font-size: 28px;
                            }
                        .button {
                            display: inline-block;
                            padding: 10px 20px;
                            background-color: #4caf50;
                            color: #ffffff;
                            text-decoration: none;
                            border-radius: 5px;
                            margin-top: 20px;
                            }
                        .button:hover {
                            background-color: #388e3c;
                        }
                        .footer {
                            background-color: #f2f2f2;
                            padding: 20px;
                            text-align: center;
                            font-size: 14px;
                            }
                    </style>
                </head>
                <body>
                    <div class='header'>
                        <h1>Royal Motors</h1>
                    </div>
                    <div class='content'>
                        <h2 style='background-color: #0d47a1; color: #ffffff; padding: 10px;'>Verify your email address</h2>
                        <p>Please click the button below to verify your email address:</p>
                        <form method='post' action='" + link + @"'>
                            <button type='submit' class='button'>Verify Email</button>
                        </form>
                    </div>
                        <div class='footer'>
                        <p>This email was sent by Royal Motors, located at Dahye, Beirut.</p>
                    </div>
                </body>
            </html>";

            Email.Email.sendEmail(account.email, "Verification Code", emailBody);
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
            if (account.verified) return RedirectToAction("verifyFail");
            if (account.verificationCode == code)
            {
                await accountInterface.VerifyAccount(email);
                return RedirectToAction("verifySuccess");
            }
            else return RedirectToAction("verifyFail");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return RedirectToAction("verifyFail");
            }
            return RedirectToAction("verifyFail");
        }

    }

    [HttpGet("verifySuccess")]
    public ActionResult verifySuccess()
    {

        string html = @"
        <html>
            <head>
	                <title>Email Verified</title>
            </head>
            <body>
	                <h1>Email Verified</h1>
	                <div style=""color: green;"">
		                <svg xmlns=""http://www.w3.org/2000/svg"" width=""50"" height=""50"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"" class=""feather feather-check-circle"">
  			            <path d=""M22 11.08V12a10 10 0 1 1-5.93-9.14""></path>
  			            <polyline points=""22 4 12 14.01 9 11.01""></polyline>
		                </svg>
		                <span style=""vertical-align: middle;"">Your email has been successfully verified.</span>
	                </div>
            </body>
        </html>";
        return new ContentResult { Content = html, ContentType = "text/html" };
    }

    [HttpGet("verifyFail")]
    public ActionResult verifyFail()
    {
        string html = @"
        <html>
            <head>
                <title>Email Verified</title>
            </head>
            <body>
                <h1>Email Verification Failed</h1>
                <div style='color: red;'>
                    <svg xmlns='http://www.w3.org/2000/svg' width='50' height='50' viewBox='0 0 24 24' fill='none' stroke='currentColor' stroke-width='2' stroke-linecap='round' stroke-linejoin='round' class='feather feather-x-circle'>
                        <circle cx='12' cy='12' r='10'></circle>
                        <line x1='15' y1='9' x2='9' y2='15'></line>
                        <line x1='9' y1='9' x2='15' y2='15'></line>
                    </svg>
                    <span style='vertical-align: middle;'>Sorry, we could not verify your email address. Either email is already verified or unavailable</span>
                </div>
            </body>
        </html>";
        return new ContentResult { Content = html, ContentType = "text/html" };
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
