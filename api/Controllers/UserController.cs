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
    [HttpPut("update/{userId}")]
    public async Task<ActionResult<MemberDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        MemberDto? memberDto = await userRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (memberDto is null)
            return BadRequest("Operation failed.");

        return memberDto;
    }
}
