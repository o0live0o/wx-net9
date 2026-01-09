using System.Data.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using wx.Core.SeedWork;
using wx.Infrastructure.Persistence.EFCore;

namespace wx.Infrastructure.Persistence.EFCore.Repositories;

public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
{
    private readonly WxContext _context;

    public EfRepository(WxContext context)
    {
        _context = context;
    }

    public async Task Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);
    }

    public async Task Add(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public async Task<IEnumerable<TEntity>> QueryAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity> QueryFirstAsync()
    {
        return await _context.Set<TEntity>().FirstOrDefaultAsync();
    }

    public async Task<TEntity> QueryFirstAsync(int id)
    {
        return await _context.Set<TEntity>().Where(p => p.Id.Equals(id)).FirstOrDefaultAsync();
    }

    public void Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> predicate)
    {
        if (@predicate is null)
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        return await _context.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public Task<TEntity> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}