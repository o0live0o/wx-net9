using MongoDB.Driver;
using System.Diagnostics.Metrics;
using wx.Core.Entities;
using wx.Infrastructure.Persistence.MongoDb.Documents;

namespace wx.Infrastructure.Persistence.MongoDb;

public class MongoDbContext
{
    private readonly IMongoDatabase _mongoDatabase;
    public MongoDbContext(IMongoDatabase mongoDatabase)
    {
        _mongoDatabase = mongoDatabase;
    }
    public IMongoCollection<Counter> Counters => _mongoDatabase.GetCollection<Counter>("Counters");

    public IMongoDatabase Database => _mongoDatabase;

    public IMongoCollection<T> GetCollection<T>()
    {
        var name = CollectionNameResolver.GetCollectionName<T>();
        return _mongoDatabase.GetCollection<T>(name);
    }

    public virtual async Task<int> GetNextIdAsync(string name)
    {
        var filter = Builders<Counter>.Filter.Eq(c => c.Id, name);
        var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
        var options = new FindOneAndUpdateOptions<Counter>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = true
        };

        var counter = await Counters.FindOneAndUpdateAsync(filter, update, options);
        return counter.SequenceValue;
    }

}
