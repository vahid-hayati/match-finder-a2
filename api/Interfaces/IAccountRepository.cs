using api.DTOs;
using api.Models;
using MongoDB.Driver;

namespace api.Interfaces;

public interface IAccountRepository
{
    public Task<LoggedInDto?> RegisterAsync(AppUser userInput, CancellationToken cancellationToken);
    public Task<LoggedInDto?> LoginAsync(LoginDto userInput, CancellationToken cancellationToken);
    public Task<List<AppUser>?> GetAllAsync(CancellationToken cancellationToken);
    public Task<MemberDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken);
    public Task<MemberDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken);
    public Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken);
}
