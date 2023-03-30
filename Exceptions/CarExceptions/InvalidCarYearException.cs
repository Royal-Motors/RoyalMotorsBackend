namespace CarWebsiteBackend.Exceptions.CarExceptions
{
    //This exception could be thrown when a car's year is not valid (e.g., negative or zero).
    public class InvalidCarYearException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "InvalidCarYearException";
            }
        }
    }
}
