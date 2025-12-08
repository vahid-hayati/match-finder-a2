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
        AppUser? appUser = await _collection.Find(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        string[]? imageUrls = await _photoService.AddPhotoToDiskAsync(file, userId);

        if (imageUrls is not null)
        {
            Photo photo = new Photo(
            Url_165: imageUrls[0],
            Url_256: imageUrls[1],
            Url_enlarged: imageUrls[2],
            IsMain: true
            );

            appUser.Photos.Add(photo);
            /*
                //     if (appUser.Photos.Count == 0)
                //     {
                //         return new Photo(
                //     Url_165: imageUrls[0],
                //     Url_256: imageUrls[1],
                //     Url_enlarged: imageUrls[2],
                //     IsMain: true
                // );
                //         // photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: true);
                //     }
                //     else
                //     {

                //                 return new Photo(
                //         Url_165: imageUrls[0],
                //         Url_256: imageUrls[1],
                //         Url_enlarged: imageUrls[2],
                //         IsMain: true
                //     );
                // photo = Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);
            */



            // photo = appUser.Photos.Count == 0
            //     ? Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: true)
            //     : Mappers.ConvertPhotoUrlsToPhoto(imageUrls, isMain: false);


            UpdateDefinition<AppUser> updatedUser = Builders<AppUser>.Update
                .Set(doc => doc.Photos, appUser.Photos);

            UpdateResult result = await _collection.UpdateOneAsync(doc => doc.Id == userId, updatedUser, null, cancellationToken);

            return result.ModifiedCount == 1 ? photo : null;
        }

        return null;

    }
}
