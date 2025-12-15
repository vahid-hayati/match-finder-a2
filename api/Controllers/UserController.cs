using api.Controllers.Helpers;
using api.DTOs;
using api.Extensions;
using api.Extensions.Validations;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update-by-id")]
    public async Task<ActionResult<MemberDto>> UpdateById(AppUser userInput, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("Login again.");

        MemberDto? memberDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (memberDto is null)
            return BadRequest("Operation failed.");

        return memberDto;
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<Photo>> AddPhoto(
        [AllowedFileExtensions, FileSize(250_000, 4_000_000)]
        IFormFile file, CancellationToken cancellationToken
    )
    {
        if (file is null) return BadRequest("No file selected with this request");

        string? userId = User.GetUserId();

        if (userId is null)
        {
            return Unauthorized("You are not logged in. please login again");
        }

        Photo? photo = await userRepository.UploadPhotoAsync(file, userId, cancellationToken);

        return photo is null ? BadRequest("Add photo failed. See logger") : photo;
    }

    // [HttpGet]
    // public string? ShowId()
    // {
    //     string? userId = User.GetUserId();

    //     return userId;
    // }
}
