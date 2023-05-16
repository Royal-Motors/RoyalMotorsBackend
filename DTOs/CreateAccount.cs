using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs
{
    public record CreateAccount
    {
        public CreateAccount([Required] string email, [Required] string password, [Required] string firstname, [Required] string lastname,
            string phoneNumber = "", string address = "")
        {
            this.email = email;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
            this.phoneNumber = phoneNumber;
            this.address = address;
        }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
    }
}