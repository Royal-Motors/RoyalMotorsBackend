using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record Account(
    [Required] Email email,
    [Required] string password,
    [Required] string firstname,
    [Required] string lastname);
