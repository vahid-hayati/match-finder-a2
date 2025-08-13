using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Repositoris;

public class AccountRepository : IAccountRepository
{
    #region dependency injections
    private readonly IMongoCollection<AppUser> _collection;
    // constructor - dependency injections
    public AccountRepository(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }
    #endregion

    public async Task<LoggedInDto?> RegisterAsync(AppUser userInput, CancellationToken cancellationToken)
    {
        AppUser user = await _collection.Find(doc
         => doc.UserName == userInput.UserName).FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
        {
            return null;
        }

        await _collection.InsertOneAsync(userInput, null, cancellationToken);

        LoggedInDto loggedInDto = new LoggedInDto(
            UserName: userInput.UserName,
            Age: userInput.Age
        );

        return loggedInDto;
    }

    public async Task<LoggedInDto?> LoginAsync(LoginDto userInput, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find(doc =>
                 doc.Password == userInput.Password && doc.UserName == userInput.UserName).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
        {
            return null;
        }

        LoggedInDto loggedRes = new LoggedInDto(
            UserName: appUser.UserName,
            Age: appUser.Age
        );

        return loggedRes;
    }

    public async Task<List<AppUser>?> GetAllAsync(CancellationToken cancellationToken)
    {
        List<AppUser> appUsers = await _collection.Find
                        (new BsonDocument()).ToListAsync(cancellationToken);

        if (appUsers.Count == 0)
            return null;

        return appUsers;
    }

    public async Task<MemberDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find
                    (doc => doc.UserName == userName).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        MemberDto memberDto = new(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: appUser.Age,
            Gender: appUser.Gender,
            City: appUser.City,
            Country: appUser.Country
        );

        return memberDto;
    }

    public async Task<MemberDto?> UpdateByIdAsync(string userId, AppUser userInput, CancellationToken cancellationToken)
    {
        UpdateDefinition<AppUser> updateDef = Builders<AppUser>.Update
                    .Set(user => user.Email, userInput.Email.Trim().ToLower());

        await _collection.UpdateOneAsync(user
            => user.Id == userId, updateDef, null, cancellationToken);

        AppUser appUser = await _collection.Find(user => user.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        MemberDto memberDto = new(
            Email: appUser.Email,
            UserName: appUser.UserName,
            Age: appUser.Age,
            Gender: appUser.Gender,
            City: appUser.City,
            Country: appUser.Country
        );

        return memberDto;
    }

    public async Task<DeleteResult?> DeleteByIdAsync(string userId, CancellationToken cancellationToken)
    {
        AppUser appUser = await _collection.Find<AppUser>(doc => doc.Id == userId).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
        {
            return null;
        }

        return await _collection.DeleteOneAsync<AppUser>(doc => doc.Id == userId, cancellationToken);
    }

}
