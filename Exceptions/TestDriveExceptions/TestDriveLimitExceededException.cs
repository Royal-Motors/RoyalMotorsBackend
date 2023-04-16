namespace CarWebsiteBackend.Exceptions.TestDriveExceptions;

//This exception can be thrown if the user exceeds the maximum number of test drives allowed per day, week, or month.
public class TestDriveLimitExceededException : Exception
{
    public TestDriveLimitExceededException(string message) : base(message) { }
    public TestDriveLimitExceededException() : base() { }
}