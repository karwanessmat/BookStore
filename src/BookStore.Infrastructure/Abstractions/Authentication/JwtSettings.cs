#pragma warning disable CS8618

namespace BookStore.Infrastructure.Abstractions.Authentication;

public class JwtSettings
{
    public static string SectionName => "JwtSettings";
    public string SecretKey { get; init; }
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }

    public string MetadataUrl { get; set; } = string.Empty;

    public bool RequireHttpsMetadata { get; init; }

}