using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using CarWebsiteBackend.Email;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using Microsoft.AspNetCore.Rewrite;
using System;
using CarWebsiteBackend.HTMLContent;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
            var account = new Account(create_account.email, BCrypt.Net.BCrypt.HashPassword(create_account.password), create_account.firstname, create_account.lastname, false, code);
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

    [HttpGet("verify/{email}/{code}")]
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
        catch
        {
            return verifyFail();
        }

    }
    [HttpGet("{email}"), Authorize]
    public async Task<ActionResult<Account>> GetAccount(string email)
    {
        if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email && emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to view this account.");
        }
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

    [HttpDelete("delete/{email}"), Authorize]
    public async Task<IActionResult> DeleteAccount(string email)
    {
        if(!IsValidEmail(email))
        {
            return BadRequest("Invalid email format.");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email && emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to delete this account.");
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

    [HttpPut("edit/{email}"), Authorize]
    public async Task<ActionResult<Account>> Edit(EditedAcc editedAcc, string email)
    {
        if (!IsValidEmail(email))
        {
            return BadRequest("Invalid email format");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email || emailClaim != "royalmotorslb@gmail.com")
        {
            return Unauthorized("You are not authorized to edit this account.");
        }
        try
        {
            Account new_acc = new Account(email, BCrypt.Net.BCrypt.HashPassword(editedAcc.password), editedAcc.firstname, editedAcc.lastname);
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
            var result = BCrypt.Net.BCrypt.Verify(password, account.password);
            if (result){
                var claims = new[] {
                        new Claim(ClaimTypes.Email, account.email)
                };
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("thisisasecretkey@123"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var jwtSecurityToken = new JwtSecurityToken(
                    issuer: "http://localhost:7284",
                    audience: "http://localhost:7284",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signinCredentials
                );
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    expiration = jwtSecurityToken.ValidTo
                });
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

