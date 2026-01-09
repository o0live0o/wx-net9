using System.Linq.Expressions;

namespace wx.Core.SeedWork;

public interface IRepository<TEntity> where TEntity : IAggregateRoot 
{
    Task<TEntity> GetByIdAsync(int id);

    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity,bool>> predicate);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
}