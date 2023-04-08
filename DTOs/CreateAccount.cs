using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs
{
    public record CreateAccount
    {
        public CreateAccount([Required] string email, [Required] string password, [Required] string firstname, [Required] string lastname)
        {
            this.email = email;
            this.password = password;
            this.firstname = firstname;
            this.lastname = lastname;
        }
        public string email { get; set; }
        public string password { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

    }
}