using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase
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
