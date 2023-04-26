using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record SignInRequest
{
    public SignInRequest([Required] string email, [Required] string password)
    {
        this.email = email;
        this.password = password;
    }

    public string email { get; set; }
    public string password { get; set; }
}
