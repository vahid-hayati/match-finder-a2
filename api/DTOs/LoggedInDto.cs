namespace api.DTOs;

public record LoggedInDto(
    string Email,
    string UserName,
    int Age,
    string Token
);