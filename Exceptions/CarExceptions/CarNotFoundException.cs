namespace CarWebsiteBackend.Exceptions.CarExceptions;
public class CarNotFoundException : Exception
{
    public CarNotFoundException(string message) : base(message) { }
    public CarNotFoundException() : base() { }
}