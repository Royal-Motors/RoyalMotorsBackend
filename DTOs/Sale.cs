using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record Sale
{
    public Sale([Required] string Email, [Required] string CarName, [Required] int Price, [Required] int Time)
    {
        this.Email = Email;
        this.CarName = CarName;
        this.Price = Price;
        this.Time = Time;
    }
    public int Id { get; set; }
    public string Email { get; set; }
    public string CarName { get; set; }
    public int Price { get; set; }
    public int Time { get; set; }
}