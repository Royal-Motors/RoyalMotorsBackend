using System.Text.RegularExpressions;
namespace CarWebsiteBackend.DTOs;

public class Email
{
    public string address { get; init; }

    public Email(string _address)
    {
        if (!IsValidEmail(_address))
        {
            throw new ArgumentException("Invalid email address");
        }
        address = _address;
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
