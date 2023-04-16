using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CarWebsiteBackend.DTOs
{
    public record Account
    {
        public Account([Required] string email, [Required] string password, [Required] string firstname, [Required] string lastname, [Required] bool verified, [Required] string verificationCode)
        {
            this.email = email;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
            this.verified = verified;
            this.verificationCode = verificationCode;
        }

        public Account([Required] string email, [Required] string password, [Required] string firstname, [Required] string lastname)
        {
            this.email = email;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
        }

        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        public bool verified { get; set; }

        public string verificationCode { get; set; }

        [JsonIgnore]
        public ICollection<TestDrive>? TestDrives { get; set; }
    }
}
