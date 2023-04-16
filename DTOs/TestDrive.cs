
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarWebsiteBackend.DTOs
{
    public record TestDrive
    {
        public TestDrive() { } // Add a default constructor for Entity Framework

        public TestDrive(TestDriveRequest request, Car car, Account account)
        {
            Time = request.Time;
            Car = car;
            Account = account;
            CarId = car.Id;
            AccountId = account.Id;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Time { get; set; }

        [ForeignKey("CarId")]
        public int CarId { get; set; }

        public Car ?Car { get; set; }

        [ForeignKey("AccountId")]
        public int AccountId { get; set; }

        public Account ?Account { get; set; }
    }
}
