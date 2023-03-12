using System.Text.RegularExpressions;
namespace CarWebsiteBackend.DTOs;

public record Email
{
    public string address { get; init; }

    public Email(string address)
    {
        if (!IsValidEmail(address))
        {
            throw new ArgumentException("Invalid email address");
        }
        this.address = address;
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
}
