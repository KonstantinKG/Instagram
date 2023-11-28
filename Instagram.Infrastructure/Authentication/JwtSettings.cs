namespace Instagram.Infrastructure.Authentication;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string AccessTokenExpiryMinutes { get; set; } = null!;
    public string RefreshTokenExpiryMinutes { get; set; } = null!;
}