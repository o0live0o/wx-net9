using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wx.Infrastructure.Persistence.MongoDb.Documents;

internal class UserRoleDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int UserId { get; set; }

    public int RoleId { get; set; }
    
}