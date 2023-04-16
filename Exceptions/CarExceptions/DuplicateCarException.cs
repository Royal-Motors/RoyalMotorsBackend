namespace CarWebsiteBackend.Exceptions.CarExceptions;
public class DuplicateCarException : Exception
{
    public DuplicateCarException(string message) : base(message) { }
    public DuplicateCarException() : base() { }
}
