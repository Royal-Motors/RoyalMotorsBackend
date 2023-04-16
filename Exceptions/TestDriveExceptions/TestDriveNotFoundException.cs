namespace CarWebsiteBackend.Exceptions.TestDriveExceptions;

public class TestDriveNotFoundException : Exception
{
    public TestDriveNotFoundException(string message) : base(message) { }
    public TestDriveNotFoundException() : base() { }
}