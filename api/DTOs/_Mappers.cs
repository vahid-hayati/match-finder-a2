using api.Models;

namespace api.DTOs;

public static class Mappers
{
    public static LoggedInDto ConvertAppUserToLoggedInDto(AppUser appUser, string tokenValue)
    {
        LoggedInDto loggedInDto = new LoggedInDto(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: appUser.Age,
            Token: tokenValue
        );

        return loggedInDto;
    }

    public static MemberDto ConvertAppUserToMemberDto(AppUser appUser)
    {
        MemberDto memberDto = new(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: appUser.Age,
            Gender: appUser.Gender,
            City: appUser.City,
            Country: appUser.Country
        );

        return memberDto;
    }
}
