using System.Linq.Expressions;

namespace wx.Core.SeedWork;

public interface IRepository<TEntity> where TEntity : IAggregateRoot 
{
    IUnitOfWork UnitOfWork { get; }

    Task<IEnumerable<TEntity>> QueryAsync();

    Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity,bool>> predicate);

    Task<TEntity> QueryFirstAsync();

    Task<TEntity> QueryFirstAsync(int id);

    void Remove(TEntity entity);

    void Add(TEntity entity);

    void Add(IEnumerable<TEntity> entities);
}