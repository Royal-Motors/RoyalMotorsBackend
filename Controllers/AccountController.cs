using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CarWebsiteBackend.Exceptions.ProfileExceptions;
using CarWebsiteBackend.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
        if(!create_account.email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        if(!create_account.password.IsValidPassword())
        {
            return BadRequest("Invalid password: at least 8 characters, 1 number, and one special character.");
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
        if (!email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
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
        if (!email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email)
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
        if (!email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email)
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
        if (!email.IsValidEmail())
        {
            return BadRequest("Invalid email format");
        }
        if (!editedAcc.password.IsValidPassword())
        {
            return BadRequest("Invalid password: at least 8 characters, 1 number, and one special character.");
        }
        string emailClaim = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
        if(emailClaim != email)
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

    [HttpGet("reset/{email}")]
    public async Task<ActionResult> ResetPasswordEmail(string email)
    {
        try
        {
            string sixDigitCode = RandomCodeGenerator();
            await accountInterface.PutForgotPasswordCode(email, sixDigitCode);
            Email.Email.sendEmail(email, "Reset Password", HTMLContent.HTMLContent.resetPasswordEmail(sixDigitCode));
            return Ok(sixDigitCode);
        }
        catch(Exception e)
        {
            if(e is ProfileNotFoundException)
            {
                return NotFound($"User with email {email} not found");
            }
            throw;
        }
    }

    [HttpPost("reset")]
    public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
    {
        try
        {
            if (!request.Password.IsValidPassword())
            {
                return BadRequest("Invalid password: at least 8 characters, 1 number, and one special character.");
            }
            await accountInterface.ResetPassword(request.Email, BCrypt.Net.BCrypt.HashPassword(request.Password), request.Code);
            return Ok("Password has been successfully reset");
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException)
            {
                return NotFound($"User with email {request.Email} not found");
            }
            throw;
        }
    }

    [HttpPost("sign_in")]
    public async Task<ActionResult<Account>> SignIn(SignInRequest request)
    {
        if(!request.email.IsValidEmail())
        {
            return BadRequest("Invalid email format.");
        }
        try{
            var account = await accountInterface.GetAccount(request.email);
            if (!account.verified)
            {
                return Unauthorized("Unverified email address");
            }
            var result = BCrypt.Net.BCrypt.Verify(request.password, account.password);
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
            else
            {
                throw new IncorrectPasswordException();
            }
        }
        catch (Exception e)
        {
            if (e is ProfileNotFoundException || e is IncorrectPasswordException)
            {
                return Unauthorized($"Email or password is incorrect. Try again or sign up.");
            }
            throw;
        }
    }

    private string RandomCodeGenerator()
    {
        Random random = new Random();
        int randomNumber = random.Next(100000, 999999);
        string sixDigitCode = randomNumber.ToString("D6");
        return sixDigitCode;
    }
}