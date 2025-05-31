#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BookStore.Application.Abstractions.Models;

public sealed class TimezoneDb
{
    public string ApiKey { get; init; }
}