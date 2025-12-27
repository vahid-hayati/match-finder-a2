using System.Runtime.InteropServices;

namespace api.DTOs;

public record RegisterDto(
    string UserName,
    string Email,
    DateOnly DateOfBirth,
    string Password,
    string ConfirmPassword
);