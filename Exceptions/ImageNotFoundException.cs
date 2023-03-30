namespace CarWebsiteBackend.Exceptions;

public class ImageNotFoundException : Exception
{
    public override string Message
    {
        get
        {
            return "Image Not Found";
        }
    }
}

