namespace BookStore.Infrastructure.Abstractions.Authentication;

#pragma warning disable CS8618
public sealed class RevokedTokenOptions
{
    public int PurgeIntervalMinutes { get; init; } = 15;
    public int BatchSize { get; init; } = 5000;
}