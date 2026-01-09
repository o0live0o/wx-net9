namespace wx.Core.SeedWork;
public interface IUnitOfWork : IDisposable
{
    Guid CurrentTransactionId { get; }
    bool HasActiveTransaction { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
    Task<TResult> ExecuteTransactionalAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default);
}
