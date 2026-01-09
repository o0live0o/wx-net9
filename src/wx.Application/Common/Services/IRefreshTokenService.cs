using wx.Core.Domain.Security;

namespace wx.Application.Common.Interfaces;

public interface IRefreshTokenService
{
    Task<string> GenerateTokenAsync(int userId, string? deviceId, string? au, string? ip);
}

public class RefreshTokenOptions
{
    public int ExpireDate { get; set; }
}