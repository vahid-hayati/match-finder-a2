using System.Runtime.InteropServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace api.Models;

public record AppUser(
    [Optional]
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)] string? Id, // hamishe sabet
    string Email,
    string UserName,
    string Password,
    string ConfirmPassword,
    DateOnly DateOfBirth,
    string Gender,
    string City,
    string Country,
    List<Photo> Photos
);

