namespace CarWebsiteBackend.Exceptions;

public class IncorrectPasswordException : Exception
{
    public IncorrectPasswordException(string message) : base(message) { }
    public IncorrectPasswordException() : base() { }
}