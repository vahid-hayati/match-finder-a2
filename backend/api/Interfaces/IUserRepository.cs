using api.DTOs;
using api.Models;
using MongoDB.Driver;

namespace api.Interfaces;

public interface IUserRepository
{
    public Task<UpdateResult?> UpdateByIdAsync(string userId, UserUpdateDto userInput, CancellationToken cancellationToken);
    public Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken);
    public Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken);
    public Task<UpdateResult?> DeletePhotoAsync(string userId, string urlIn, CancellationToken cancellationToken);
}
