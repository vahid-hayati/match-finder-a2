using api.DTOs;
using api.Models;
using MongoDB.Driver;

namespace api.Interfaces;

public interface IUserRepository
{
    public Task<MemberDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken);
    public Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken);
    public Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken);
}
