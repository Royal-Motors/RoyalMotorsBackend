
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarWebsiteBackend.DTOs
{
    public record TestDrive
    {
        public TestDrive() { } // Add a default constructor for Entity Framework

        public TestDrive([Required] int Time, [Required] int CarId, [Required] int AccountId)
        {
            this.Time = Time;
            this.AccountId = AccountId;
            this.CarId = CarId;
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
