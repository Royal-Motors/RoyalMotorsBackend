namespace CarWebsiteBackend.Exceptions
{
    public class ProfileNotFound: Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "ProfileNotFound";
            }
        }
    }
}
