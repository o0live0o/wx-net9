namespace wx.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<TokenDto> GetTokenAsync(string username, string password,string remoteIp, string? deviceId);

    Task<TokenDto> RefreshTokenAsync(string refreshToken);
}

public record TokenDto(string TokenType, string AccessToken, string RefreshToken, DateTime ExpireAt,string deviceId);

public static class IdentityConstant
{
    public const string Bearer = "bearer";
}
    