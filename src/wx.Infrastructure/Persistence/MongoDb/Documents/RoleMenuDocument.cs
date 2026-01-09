using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wx.Infrastructure.Persistence.MongoDb.Documents;

internal class RoleMenuDocument
{
    [BsonId]
    public ObjectId Id { get; set; }

    public int RoleId { get; set; }

    public int MenuId { get; set; }
}
