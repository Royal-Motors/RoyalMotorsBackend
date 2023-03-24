using System.Reflection;

namespace CarWebsiteBackend.Exceptions
{
    //This exception could be thrown when a test drive request is made for a car that is not available for test drive.
    public class InvalidTestDriveRequestException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "InvalidTestDriveRequestException";
            }
        }
    }
}

