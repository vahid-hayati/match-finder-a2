using api.Controllers.Helpers;
using api.DTOs;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Authorize]
public class MemberController(IMemberRepository memberRepository) : BaseApiController
{
    [HttpGet("get-all")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        IEnumerable<AppUser> appUsers = await memberRepository.GetAllAsync(cancellationToken);

        if (appUsers.Count() == 0)
            return NoContent();

        List<MemberDto> memberDtos = [];

        foreach (AppUser user in appUsers)
        {
            MemberDto memberDto = Mappers.ConvertAppUserToMemberDto(user);

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