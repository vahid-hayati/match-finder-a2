using api.DTOs;
using api.Models;

namespace api.Interfaces;

public interface IUserRepository
{
    public Task<MemberDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken);
    // public Task<LoggedInDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken);
}
