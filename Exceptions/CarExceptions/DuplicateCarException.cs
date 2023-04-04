using System.Reflection;

namespace CarWebsiteBackend.Exceptions.CarExceptions;

//This exception could be thrown when the car object being added already exists in the car dealership's inventory.
public class DuplicateCarException : Exception
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

