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