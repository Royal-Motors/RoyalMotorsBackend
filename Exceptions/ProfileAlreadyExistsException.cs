namespace CarWebsiteBackend.Exceptions
{
    public class ProfileAlreadyExistsException: Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "ProfileAlreadyExists";
            }
        }
    }
}

