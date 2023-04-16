using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarWebsiteBackend.DTOs;

public record TestDriveRequest
{
    public TestDriveRequest([Required] int Time, [Required] string CarName, [Required] string AccountEmail)
    {
        this.Time = Time;
        this.AccountEmail = AccountEmail;
        this.CarName = CarName;
    }
    public int Time { get; set; }
    public string CarName { get; set; }
    public string AccountEmail { get; set; }

}
