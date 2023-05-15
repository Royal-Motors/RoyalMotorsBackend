using System.Text.RegularExpressions;

namespace CarWebsiteBackend.Extensions;

public static class StringExtension
{
    public static bool IsValidEmail(this string email)
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

    public static bool IsValidPassword(this string password)
    {

        try
        {
            return Regex.IsMatch(password,
                @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }

    public static bool IsPhoneNumberValid(this string phoneNumber)
    {
        try
        {
            return Regex.IsMatch(phoneNumber,
                @"^\+?\d{1,4}?\s?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(250));
        }
        catch (RegexMatchTimeoutException)
        {
            return false;
        }
    }
}
