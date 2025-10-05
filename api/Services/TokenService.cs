using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Interfaces;
using api.Models;
using api.Settings;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace api.Services;

public class TokenService : ITokenService
{
    private readonly IMongoCollection<AppUser> _collection;
    private readonly SymmetricSecurityKey? _key;

    public TokenService(IConfiguration config, IMongoClient client, IMongoDbSettings dbSettings)
    {
        var database = client.GetDatabase(dbSettings.DatabaseName);
        _collection = database.GetCollection<AppUser>("users");

        string? tokenValue = config.GetValue<string>("TokenKey");

        _ = tokenValue ?? throw new ArgumentException("token key cannot be null", nameof(tokenValue));

        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue!));
    }

    public string CreateToken(AppUser appUser)
    {
        _ = _key ?? throw new ArgumentException("_key cannot be null", nameof(_key));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, appUser.Id!),
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
        };

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(2),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}