using api.Models;

namespace api.DTOs;

public record MemberDto(
    string Email,
    string UserName,
    int Age,
    string Gender,
    string City,
    string Country,
    List<Photo> Photos
);