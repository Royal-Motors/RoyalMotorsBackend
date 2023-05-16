using System.ComponentModel.DataAnnotations;

namespace CarWebsiteBackend.DTOs;

public record DeleteTestDriveRequest(string reason = "");