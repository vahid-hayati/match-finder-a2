using api.Controllers.Helpers;
using api.DTOs;
using api.Extensions;
using api.Extensions.Validations;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers;

[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update-by-id")]
    public async Task<ActionResult<Response>> UpdateById(UserUpdateDto userInput, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("Login again.");

        UpdateResult? result = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (result is null)
            return BadRequest("Operation failed.");

        Response res = new Response(
            Message: "User has been updated successfully."
        );

        return Ok(res);
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

    [HttpPut("set-main-photo")] 
    public async Task<ActionResult<Response>> SetMainPhoto(string photoUrlIn, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
        {
            return Unauthorized("You are not logged in. please login again");
        }

        UpdateResult? updateResult = await userRepository.SetMainPhotoAsync(userId, photoUrlIn, cancellationToken);

        // Response response = new Response(
        //     Message: "Set this photo as main succeeded."
        // );

        return updateResult is null || !updateResult.IsModifiedCountAvailable
            ? BadRequest("Set as main photo failed. Try again in a few moments. If the issue persists contact the admin.")
            : Ok(new Response(
                Message: "Set this photo as main succeeded."
            ));
    }

    [HttpPut("delete-photo")]
    public async Task<ActionResult<Response>> DeletePhoto(string photoUrlIn, CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (string.IsNullOrEmpty(userId))
            return Unauthorized("The user is not logged in");

        UpdateResult? result = await userRepository.DeletePhotoAsync(userId, photoUrlIn, cancellationToken);

        return result is null || !result.IsModifiedCountAvailable
            ? BadRequest("Photo deletion failed. Try again in a few moments. If the issue persists contact the admin.")
            : Ok(new Response(
                Message: "Photo deleted successfully."
            ));
    }

    // [HttpGet]
    // public string? ShowId()
    // {
    //     string? userId = User.GetUserId();

    //     return userId;
    // }
}
