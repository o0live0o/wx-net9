namespace wx.Core.Domain.Security;

public class RefreshTokenRecord : Entity, IAggregateRoot
{
    public string TokenHash { get; private set; }

    public string DeviceId { get; private set; }

    public int UserId { get; private set; }

    public string? CreatedByIp { get; private set; }

    public string? Au { get; private set; }

    public DateTime ExpireAt { get; private set; }

    public bool IsRevoked { get; private set; } = false;

    private RefreshTokenRecord() { }

    public static RefreshTokenRecord Create(string tokenHash, int userId, string deviceId, string? ip, string? au)
    {
        return new RefreshTokenRecord() { TokenHash = tokenHash, UserId = userId, DeviceId = deviceId, CreatedByIp = ip, Au = au };
    }

    public void SetExpireTime(int day)
    {
        ExpireAt = DateTime.UtcNow.AddDays(day);
    }

    public void RevokeToken()
    {
        IsRevoked = true;
    }
}
