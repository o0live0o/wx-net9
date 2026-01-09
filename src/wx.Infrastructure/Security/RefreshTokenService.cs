using wx.Application.Common.Interfaces;
using wx.Core.Domain.Security;
using wx.Core.IRepositories;
using wx.Infrastructure.Utils;

namespace wx.Infrastructure.Security;

[AutoRegister]
public class RefreshTokenService(IRefreshTokenRecordRepository repository) : IRefreshTokenService
{
    public async Task<string> GenerateTokenAsync(int userId, string? deviceId, string? au, string? ip)
    {
        var refreshToken = HashUtils.GenarateHash();
        var tokenHash = HashUtils.HashString(refreshToken);
        var record = RefreshTokenRecord.Create(tokenHash, userId, deviceId, ip, au);
        await repository.SaveRrfreshTokenAsync(record);
        return refreshToken;
    }
}
