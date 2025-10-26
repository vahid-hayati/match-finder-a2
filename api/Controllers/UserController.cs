using api.Controllers.Helpers;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
{
    [HttpPut("update-by-id/{userId}")]
    public async Task<ActionResult<MemberDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    // public async Task<ActionResult<LoggedInDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        MemberDto? memberDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);
        // LoggedInDto? loggedInDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        // if (loggedInDto is null)
        //     return BadRequest("Operation failed.");

        // return loggedInDto;
        if (memberDto is null)
            return BadRequest("Operation failed.");

        return memberDto;
    }
}
