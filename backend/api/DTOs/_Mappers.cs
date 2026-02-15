// using api.Extensions;
using api.Extensions;
using api.Models;

namespace api.DTOs;

public static class Mappers
{
    public static AppUser ConvertRegisterDtoToAppUser(RegisterDto registerDto)
    {
        AppUser appUser = new AppUser(
           Email: registerDto.Email,
           UserName: registerDto.UserName,
           Password: registerDto.Password,
           ConfirmPassword: registerDto.ConfirmPassword,
           DateOfBirth: registerDto.DateOfBirth,
           Gender: registerDto.Gender, 
           Introduction: string.Empty, // ""
           LookingFor: string.Empty,
           Interests: string.Empty,
           City: string.Empty,
           Country: string.Empty,
           LastActive: DateTime.UtcNow,
           Photos: []
       );

        return appUser;
    }

    public static LoggedInDto ConvertAppUserToLoggedInDto(AppUser appUser, string tokenValue)
    {
        LoggedInDto loggedInDto = new LoggedInDto(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: DateTimeExtensions.CalculateAge(appUser.DateOfBirth),
            ProfilePhotoUrl: appUser.Photos.FirstOrDefault(photo => photo.IsMain)?.Url_165,
            Token: tokenValue
        );

        return loggedInDto;
    }

    public static MemberDto ConvertAppUserToMemberDto(AppUser appUser)
    {
        MemberDto memberDto = new(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: DateTimeExtensions.CalculateAge(appUser.DateOfBirth),
            Introduction: appUser.Introduction,
            LookingFor: appUser.LookingFor,
            Interests: appUser.Interests,
            Gender: appUser.Gender,
            City: appUser.City,
            Country: appUser.Country,
            Photos: appUser.Photos
        );

        return memberDto;
    }

    public static Photo ConvertPhotoUrlsToPhoto(string[] photoUrls, bool isMain)
    {
        Photo photo = new Photo(
            Url_165: photoUrls[0],
            Url_256: photoUrls[1],
            Url_enlarged: photoUrls[2],
            IsMain: isMain
        );

        return photo;
    }
}
