namespace CarWebsiteBackend.Exceptions;

public class ImageNotFoundException : Exception
{
    public ImageNotFoundException(string message) : base(message) { }
    public ImageNotFoundException() : base() { }
}