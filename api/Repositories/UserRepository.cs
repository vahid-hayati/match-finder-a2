using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using MongoDB.Driver;

namespace api.Repositoris;

public class UserRepository : IUserRepository
{
    #region dependency injections
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;
    private readonly IPhotoService _photoService;
    private readonly ILogger<UserRepository> _logger;

    // constructor - dependency injections
    public UserRepository(IMongoClient client, IMongoDbSettings dbSettings, ITokenService tokenService, IPhotoService photoService, ILogger<UserRepository> logger)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
        _photoService = photoService;
        _logger = logger;
    }
    #endregion

    public async Task<AppUser?> GetByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        return appUser;
    }

    public async Task<MemberDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
                    .Set(user => user.Email, userInput.Email.Trim().ToLower())
                    .Set(user => user.UserName, userInput.UserName.Trim().ToLower());

        await _collection.UpdateOneAsync(user
            => user.Id == userId, updateDef, null, cancellationToken);

        AppUser appUser = await _collection.Find(user => user.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        MemberDto memberDto = Mappers.ConvertAppUserToMemberDto(appUser);

        return memberDto;
    }

    public async Task<Photo?> UploadPhotoAsync(IFormFile file, string userId, CancellationToken cancellationToken)
    {
        Photo photo;

        AppUser? appUser = await GetByIdAsync(userId, cancellationToken);

        if (appUser is null)
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, userId);
        //["filePath_165_sq", "filePath_256_sq", "filePath_enlarged"] 

        if (imageUrls is not null)
        {
            #region old sintax
            // {
            //     photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, true);
            // }
            // else
            // {
            //     photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, false);
            // }
            #endregion

            photo = appUser.Photos.Count == 0
                 ? Mappers.ConvertPhotoUrlsToPhoto(imageUrls, true)
                 : Mappers.ConvertPhotoUrlsToPhoto(imageUrls, false);

            appUser.Photos.Add(photo);

            UpdateDefinition<AppUser> updatedUser = Builders<AppUser>.Update
                .Set(doc => doc.Photos, appUser.Photos);

            UpdateResult result = await _collection.UpdateOneAsync(doc => doc.Id == userId, updatedUser, null, cancellationToken);

            // if (result.ModifiedCount == 1)
            //     return photo;

            // return null;

             return result.ModifiedCount == 1 ? photo : null;
        }

        return null;
    }

    public string CheckAge()
    {
        int age = 18;

        // if (age >= 18)
        //     return "Shoma balaye 18 sal sen darid";
        // else if (age < 18)
        //     return "Shoma zire 18 sal sen darid";
        // else if (age == 20)
        //     return "Shoma 20 sal sen darid";
        // else 
        //     return "bye bye";

        return age >= 18
            ? "Shoma balaye 18 sal sen darid"
            : age < 18
            ? "Shoma zire 18 sal sen darid"
            : age == 20
            ? "Shoma 20 sal sen darid"
            : "bye bye";
    }
}