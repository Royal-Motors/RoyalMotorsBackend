namespace CarWebsiteBackend.Exceptions.TestDriveExceptions
{
    // This exception can be thrown if there is a conflict with the requested test drive
    public class TestDriveConflictException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "Test Drive Conflict";
            }
        }
    }
}

