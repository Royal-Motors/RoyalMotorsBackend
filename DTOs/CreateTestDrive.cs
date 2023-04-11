using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public class CreateTestDrive
{
    public int Time { get; set; }
    public string CarName { get; set; }
    public string Email { get; set; }

    public CreateTestDrive([Required] int Time, [Required] string CarName, [Required] string Email)
    {
        this.Time = Time;
        this.CarName = CarName;
        this.Email = Email;
    }
}