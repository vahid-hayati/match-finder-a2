using api.Models;

namespace api.DTOs;

public static class Mappers
{
    public static LoggedInDto ConvertAppUserToLoggedInDto(AppUser appUser, string token)
    {
        LoggedInDto loggedInDto = new LoggedInDto(
            UserName: appUser.UserName,
            Age: appUser.Age,
            Token: token,
            City: appUser.City
        );

        return loggedInDto;
    }
}
