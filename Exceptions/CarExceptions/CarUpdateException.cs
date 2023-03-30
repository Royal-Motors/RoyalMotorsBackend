using System.Reflection;

namespace CarWebsiteBackend.Exceptions
{
    //This exception could be thrown when an error occurs while updating a car's information in the inventory.
    //Like missing required field
    public class CarUpdateException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "DuplicateCarException";
            }
        }
    }
}

