using System.Collections.Concurrent;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;

namespace wx.Infrastructure.Caching;

[AutoRegister]
public class InMemoryRefreshTokenRepository : IRefreshTokenRecordRepository
{
    public static readonly ConcurrentDictionary<string, RefreshTokenRecord> _dic = new ConcurrentDictionary<string, RefreshTokenRecord>();

    public Task<RefreshTokenRecord?> GetRefreshTokenAsync(string tokenHash)
    {
        _dic.TryGetValue(tokenHash,out var record);
        return Task.FromResult(record);
    }

    public Task SaveRrfreshTokenAsync(RefreshTokenRecord record)
    {
        _dic.TryAdd(record.TokenHash, record);
        return Task.CompletedTask;
    }
}
