using System.Reflection;

namespace CarWebsiteBackend.Exceptions
{
    //This exception could be thrown when the car object is not valid or is missing required fields.
    //Might be useful when the dealership is trying to add new car but with missing required fileds.
    public class InvalidCarException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "InvalidCarException";
            }
        }
    }
}

