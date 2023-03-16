using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record EditedAcc(
    [Required] string password,
    [Required] string firstname,
    [Required] string lastname);