using api.Controllers.Helpers;
using api.DTOs;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace api.Controllers;

public class AccountController(IAccountRepository accountRepository) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<LoggedInDto>> Register(RegisterDto userInput, CancellationToken cancellationToken)
    {
        if (userInput.Password != userInput.ConfirmPassword)
            return BadRequest("Your passwords do not match!");

        LoggedInDto? loggedInDto = await accountRepository.RegisterAsync(userInput, cancellationToken);

        if (loggedInDto is null)
            return BadRequest("This user name is already taken.");

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

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<LoggedInDto>> ReloadLoggedInUser(CancellationToken cancellationToken)
    {
        string? token = null;

        bool isTokenValid =
            HttpContext.Request.Headers.
            TryGetValue("Authorization", out var authHeader);

        // Console.WriteLine(authHeader);

        if (isTokenValid)
            token = authHeader.ToString().Split(' ').Last();

        Console.WriteLine(token);

        if (string.IsNullOrEmpty(token))
            return Unauthorized("Token is expired or invalid. Login again.");

        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized();

        LoggedInDto? loggedInDto =
       await accountRepository.ReloadLoggedInUserAsync(userId, token, cancellationToken);

        if (loggedInDto is null)
            return Unauthorized("User is logged out or unauthorized. Login again");

        return loggedInDto;

        // return loggedInDto is null ? Unauthorized("User is logged out or unauthorized. Login again") : loggedInDto;
    }

    [Authorize]
    [HttpDelete("delete-by-id")]
    public async Task<ActionResult<DeleteResult>> DeleteById(CancellationToken cancellationToken)
    {
        string? userId = User.GetUserId();

        if (userId is null)
            return Unauthorized("Please login again");

        DeleteResult? deleteResult = await accountRepository.DeleteByIdAsync(userId, cancellationToken);

        if (deleteResult is null)
            return BadRequest("Operation failed");

        return deleteResult;
    }

    [HttpGet("test")]
    public void TestSplit()
    {
        string name = "vahid";

        string splitedName = name.Split('a').First();

        Console.WriteLine(splitedName);
    }
}
