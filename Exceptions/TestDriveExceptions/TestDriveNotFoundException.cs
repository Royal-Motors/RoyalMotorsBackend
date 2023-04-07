namespace CarWebsiteBackend.Exceptions.TestDriveExceptions
{
    //This exception can be thrown if the requested test drive cannot be found.
    //For example if the test drive has already been completed, cancelled, or does not exist.
    public class TestDriveNotFoundException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "Test Drive Not Found";
            }
        }
    }
}
