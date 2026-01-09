using wx.Core.Domain.Security;

namespace wx.Core.IRepositories;

public interface IRefreshTokenRecordRepository
{
    Task SaveRrfreshTokenAsync(RefreshTokenRecord record);
    Task<RefreshTokenRecord?> GetRefreshTokenAsync(string tokenHash);
}
