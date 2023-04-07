namespace CarWebsiteBackend.Exceptions.TestDriveExceptions
{
    //This exception can be thrown if the user submits an invalid or incomplete test drive request
    public class InvalidTestDriveRequestException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "Invalid Test Drive Request";
            }
        }
    }
}

