
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebsiteBackend.DTOs
{
    public record TestDrive
    {
        public TestDrive() { } // Add a default constructor for Entity Framework

        public TestDrive([Required] int Time, [Required] Car Car, [Required] Account Account)
        {
            this.Time = Time;
            this.Account = Account;
            this.Car = Car;
            CarId = Car.Id;
            AccountId = Account.Id;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int Time { get; set; }

        [ForeignKey("CarId")]
        public int CarId { get; set; }
        public Car Car;

        [ForeignKey("AccountId")]
        public int AccountId { get; set; }
        public Account Account;
    }
}
