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

    // constructor - dependency injections
    public UserRepository(IMongoClient client, IMongoDbSettings dbSettings, ITokenService tokenService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");

        _tokenService = tokenService;
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

}
