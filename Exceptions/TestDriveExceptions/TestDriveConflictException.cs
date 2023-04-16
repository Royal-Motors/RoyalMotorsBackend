namespace CarWebsiteBackend.Exceptions.TestDriveExceptions;

public class TestDriveConflictException : Exception
{
    public TestDriveConflictException(string message) : base(message) {}
    public TestDriveConflictException() : base() {}
}
