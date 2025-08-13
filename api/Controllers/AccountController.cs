using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountRepository accountRepository) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<LoggedInDto>> Register(AppUser userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.RegisterAsync(userInput, cancellationToken);

        if (loggedInDto is null)
            return BadRequest("This email is already taken.");

        return Ok(loggedInDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoggedInDto>> Login(LoginDto userInput, CancellationToken cancellationToken)
    {
        LoggedInDto? loggedInDto = await accountRepository.LoginAsync(userInput, cancellationToken);

        if (loggedInDto is null)
            return BadRequest("Email or Password is wrong");

        return loggedInDto;
    }

    [HttpGet("get-all")]
    public async Task<ActionResult<List<MemberDto>>> GetAll(CancellationToken cancellationToken)
    {
        List<AppUser>? appUsers = await accountRepository.GetAllAsync(cancellationToken);

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
        MemberDto? memberDto = await accountRepository.GetByUserNameAsync(userName, cancellationToken);

        if (memberDto is null)
            return BadRequest("User not found");

        return memberDto;
    }

    [HttpPut("update/{userId}")]
    public async Task<ActionResult<MemberDto>> UpdateById(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        MemberDto? memberDto = await accountRepository.UpdateByIdAsync(userId, userInput, cancellationToken);

        if (memberDto is null)
            return BadRequest("Operation failed.");

        return memberDto;
    }

    [HttpDelete("delete/{userId}")]
    public async Task<ActionResult<DeleteResult>> DeleteById(string userId, CancellationToken cancellationToken)
    {
        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }

}
