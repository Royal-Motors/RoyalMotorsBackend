using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record ResetPasswordRequest
{
    public ResetPasswordRequest([Required] string email, [Required] string password, [Required] string code)
    {
        Email = email;
        Password = password;
        Code = code;
    }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Code { get; set; }
}