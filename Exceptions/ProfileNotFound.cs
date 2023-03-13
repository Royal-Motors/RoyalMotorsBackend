namespace CarWebsiteBackend.Exceptions
{
    public class ProfileNotFoundException: Exception
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
