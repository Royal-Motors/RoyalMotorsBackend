namespace CarWebsiteBackend.Exceptions.CarExceptions
{
    //This exception could be thrown when an error occurs while deleting a car from the car dealership's inventory.
    public class CarDeletionException : Exception
    {
        //Overriding the Message property
        public override string Message
        {
            get
            {
                return "CarDeletionException";
            }
        }
    }
}
