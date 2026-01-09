using MongoDB.Bson.Serialization.Attributes;

namespace wx.Infrastructure.Persistence.MongoDb.Documents;

public class Counter
{
    [BsonId]
    public string Id { get; set; }
    public int SequenceValue { get; set; }
}