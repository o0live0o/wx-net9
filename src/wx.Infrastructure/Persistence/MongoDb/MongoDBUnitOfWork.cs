using wx.Core.SeedWork;
using wx.Infrastructure.Services;

[AutoRegister]
public class MongoDBUnitOfWork : IUnitOfWork
{
    private Guid _transactionId;
    public MongoDBUnitOfWork()
    {
        _transactionId = Guid.CreateVersion7();
    }
    public Guid CurrentTransactionId => _transactionId;

    public bool HasActiveTransaction =>true;

    public void Dispose()
    {
        
    }

    public async Task<TResult> ExecuteTransactionalAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken = default)
    {
        var result = await operation();
        return  result;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return 1;
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        return true;
    }
}