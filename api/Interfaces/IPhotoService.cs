namespace api.Interfaces;

public interface IPhotoService
{
    public Task<string[]?> AddPhotoToDiskAsync(IFormFile file, string userId);
}
