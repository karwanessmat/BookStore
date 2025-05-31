namespace BookStore.SharedKernel.Abstractions.IServices;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    public DateTimeOffset DefaultUtcNow { get; }
    Task<DateTimeOffset> GetCurrentDateTimeOffsetByTimeZone(string? timeZoneId =null);
    DateTimeOffset ConvertOfficeDateTimeToUniversal(DateTimeOffset officeDateTime);
}


