
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
            CarName = request.CarName;
            AccountEmail = request.AccountEmail;
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
        public int CarId;
        public string CarName { get; set; }

        [JsonIgnore]
        public Car ?Car { get; set; }

        [ForeignKey("AccountId")]
        public int AccountId { get; set; }
        public string AccountEmail { get; set; }

        [JsonIgnore]
        public Account ?Account { get; set; }
    }
}
