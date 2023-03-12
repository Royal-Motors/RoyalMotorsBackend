using CarWebsiteBackend.DTOs;
using CarWebsiteBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        try
        {
            await accountInterface.AddAccount(account);
            return CreatedAtAction(nameof(SignUp), new { email = account.email }, account);
        }
        catch{
            return BadRequest("Email already taken. Sign up with another email, or sign in.");
        }
    }
}