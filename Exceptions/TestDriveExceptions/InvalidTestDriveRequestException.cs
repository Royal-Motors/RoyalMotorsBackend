namespace CarWebsiteBackend.Exceptions.TestDriveExceptions;

//This exception can be thrown if the user exceeds the maximum number of test drives allowed per day, week, or month.
public class InvalidTestDriveRequestException : Exception
{
    public InvalidTestDriveRequestException(string message) : base(message) { }
    public InvalidTestDriveRequestException() : base() { }
}