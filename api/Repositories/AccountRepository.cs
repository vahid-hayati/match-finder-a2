using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using MongoDB.Driver;

namespace api.Repositoris;

public class AccountRepository : IAccountRepository
{
    #region dependency injections
    private readonly IMongoCollection<AppUser> _collection;
    private readonly ITokenService _tokenService;

    // constructor - dependency injections
    public AccountRepository(IMongoClient client, IMongoDbSettings dbSettings, ITokenService tokenService)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
        _tokenService = tokenService;
    }
    #endregion

    public async Task<LoggedInDto?> RegisterAsync(AppUser userInput, CancellationToken cancellationToken)
    {
        AppUser? user = await _collection.Find(doc
         => doc.UserName == userInput.UserName).FirstOrDefaultAsync(cancellationToken);

        if (user is not null)
        {
            return null;
        }

        await _collection.InsertOneAsync(userInput, null, cancellationToken);

        string token = _tokenService.CreateToken(userInput);

        LoggedInDto loggedInDto = Mappers.ConvertAppUserToLoggedInDto(userInput, token);

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

        string token = _tokenService.CreateToken(appUser);

        LoggedInDto loggedInDto = Mappers.ConvertAppUserToLoggedInDto(appUser, token);

        return loggedInDto;
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
