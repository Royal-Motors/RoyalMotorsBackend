namespace CarWebsiteBackend.Exceptions.CarExceptions
{
    //This exception could be thrown when the car object is not found in the car dealership's inventory.
    public class CarNotFoundException: Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "CarNotFoundException";
            }
        }
    }
}
