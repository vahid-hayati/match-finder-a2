using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

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

    public async Task<UpdateResult?> UpdateByIdAsync(string userId, UserUpdateDto userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
                .Set(appUser => appUser.Introduction, userInput.Introduction.Trim())
                .Set(appUser => appUser.LookingFor, userInput.LookingFor.Trim())
                .Set(appUser => appUser.Interests, userInput.Interests.Trim())
                .Set(appUser => appUser.City, userInput.City.Trim().ToLower())
                .Set(appUser => appUser.Country, userInput.Country.Trim().ToLower());

        UpdateResult? result = await _collection.UpdateOneAsync(user
            => user.Id == userId, updateDef, null, cancellationToken);

        if (!result.IsModifiedCountAvailable)
            return null;

        return result;
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

    public string CheckAge() //Example for the ternary operator
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

    public async Task<UpdateResult?> SetMainPhotoAsync(string userId, string photoUrlIn, CancellationToken cancellationToken)
    {
        #region  UNSET the previous main photo: Find the photo with IsMain True; update IsMain to False
        // set query
        FilterDefinition<AppUser>? filterOld = Builders<AppUser>.Filter
            .Where(appUser =>
                appUser.Id == userId && appUser.Photos.Any<Photo>(photo => photo.IsMain == true));

        UpdateDefinition<AppUser>? updateOld = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, false);

        // UpdateOneAsync(appUser => appUser.Photos.IsMain, false, null, cancellationToken);
        await _collection.UpdateOneAsync(filterOld, updateOld, null, cancellationToken);
        #endregion

        #region  SET the new main photo: find new photo by its Url_165; update IsMain to True
        FilterDefinition<AppUser>? filterNew = Builders<AppUser>.Filter
            .Where(appUser =>
                appUser.Id == userId && appUser.Photos.Any<Photo>(photo => photo.Url_165 == photoUrlIn));

        UpdateDefinition<AppUser>? updateNew = Builders<AppUser>.Update
            .Set(appUser => appUser.Photos.FirstMatchingElement().IsMain, true);

        return await _collection.UpdateOneAsync(filterNew, updateNew, null, cancellationToken);
        #endregion
    }

    public async Task<UpdateResult?> DeletePhotoAsync(string userId, string url_165_In, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(url_165_In)) return null;

        Photo photo = await _collection.AsQueryable()
            .Where(appUser => appUser.Id == userId) // filter by user Id
            .SelectMany(appUser => appUser.Photos) // flatten the Photos array
            .Where(photo => photo.Url_165 == url_165_In) // filter by photo url
            .FirstOrDefaultAsync(cancellationToken); // return the photo or null

        if (photo is null) return null;

        if (photo.IsMain) return null;

        bool isDeleteSuccess = await _photoService.DeletePhotoFromDiskAsync(photo);

        if (!isDeleteSuccess)
        {
            _logger.LogError("Delete Photo form disk failed");

            return null;
        }

        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
            .PullFilter(appUser => appUser.Photos, photo => photo.Url_165 == url_165_In);

        return await _collection.UpdateOneAsync(appUser => appUser.Id == userId, updateDef, null, cancellationToken);
    }
}