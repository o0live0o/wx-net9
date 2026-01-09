using MongoDB.Driver;
using System.Linq.Expressions;
using wx.Core.SeedWork;
using wx.Infrastructure.Persistence.MongoDb.Documents;
using wx.Infrastructure.Services;

namespace wx.Infrastructure.Persistence.MongoDb.Repositories;

[AutoRegister]
public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    private readonly MongoDbContext _dbContext;
    protected readonly IMongoCollection<TEntity> _collection;

    public MongoRepository(MongoDbContext dbContext)
    {
        _dbContext = dbContext;
        _collection = _dbContext.GetCollection<TEntity>();

    }
    public IUnitOfWork UnitOfWork => null;

    public virtual async Task<TEntity> GetByIdAsync(int id)
    {
        return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {   
        var filter = Builders<TEntity>.Filter.Where(predicate);
        var result = await _collection.FindAsync(filter);
        return await result.ToListAsync();
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var id = await _dbContext.GetNextIdAsync(CollectionNameResolver.GetCollectionName<TEntity>());
        entity.Id = id;
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _collection.DeleteOneAsync(x => x.Id == entity.Id);
    }

    public virtual async Task UpdateAsync(TEntity entity, CancellationToken cancellationToke = default)
    {
        await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
    }
}

