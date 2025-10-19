namespace api.DTOs;

public record LoggedInDto(
    string UserName,
    int Age,
    string Token,
    string City
);