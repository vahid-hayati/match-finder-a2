using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController(IMemberRepository memberRepository) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<ActionResult<List<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        List<AppUser>? appUsers = await memberRepository.GetAllAsync(cancellationToken);

        if (appUsers is null)
            return NoContent();

        List<MemberDto> memberDtos = [];

        foreach (AppUser user in appUsers)
        {
            MemberDto memberDto = new(
                Email: user.Email,
                UserName: user.UserName,
                Age: user.Age,
                Gender: user.Gender,
                City: user.City,
                Country: user.Country
            );

            memberDtos.Add(memberDto);
        }

        return memberDtos;
    }

    [HttpGet("get-by-username/{username}")]
    public async Task<ActionResult<MemberDto?>> GetByUserName(string userName, CancellationToken cancellationToken)
    {
        MemberDto? memberDto = await memberRepository.GetByUserNameAsync(userName, cancellationToken);

        if (memberDto is null)
            return BadRequest("User not found");

        return memberDto;
    }

}