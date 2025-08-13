namespace api.DTOs;

public record LoginDto(
    string UserName,
    string Password
);