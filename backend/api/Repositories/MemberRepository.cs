using api.DTOs;
using api.Interfaces;
using api.Models;
using api.Settings;
using MongoDB.Bson;
using MongoDB.Driver;

namespace api.Repositoris;

public class MemberRepository : IMemberRepository
{
    #region dependency injections
    private readonly IMongoCollection<AppUser> _collection;
    // constructor - dependency injections
    public MemberRepository(IMongoClient client, IMongoDbSettings dbSettings)
    {
        var dbName = client.GetDatabase(dbSettings.DatabaseName);
        _collection = dbName.GetCollection<AppUser>("users");
    }
    #endregion

    public async Task<IEnumerable<AppUser>> GetAllAsync(CancellationToken cancellationToken)
    {
        IEnumerable<AppUser> appUsers = await _collection.Find
                (new BsonDocument()).ToListAsync(cancellationToken); // [] or [...]

        return appUsers;
    }

    public async Task<MemberDto?> GetByUserNameAsync(string userName, CancellationToken cancellationToken)
    {
        AppUser? appUser = await _collection.Find(doc =>
            doc.UserName == userName).FirstOrDefaultAsync(cancellationToken);

        if (appUser is null)
            return null;

        MemberDto memberDto = Mappers.ConvertAppUserToMemberDto(appUser);

        return memberDto;
    }
}
