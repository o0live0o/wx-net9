namespace wx.Infrastructure.Options;

public class TokenOptions
{
    public const string SectionName = "TokenOptions";
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecretKey { get; set; } = default!;
    public int ExpireMinutes { get; set; } = 60;
}
