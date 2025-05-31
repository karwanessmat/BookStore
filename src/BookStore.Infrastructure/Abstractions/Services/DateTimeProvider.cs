using BookStore.Infrastructure.Abstractions.Helpers;
using BookStore.SharedKernel.Abstractions.IServices;

namespace BookStore.Infrastructure.Abstractions.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;          // DateTime.Kind = Utc
    public DateTimeOffset DefaultUtcNow => DateTimeHelper.GetCurrentDateTimeOffsetByTimeZone().Result;
    
    

    public async Task<DateTimeOffset> GetCurrentDateTimeOffsetByTimeZone(string? timeZoneId = null)
    {
        var result = await DateTimeHelper.GetCurrentDateTimeOffsetByTimeZone(timeZoneId);
        return result;
    }

    // Converts a given office DateTimeOffset to UTC (with an offset of +00:00)
    public DateTimeOffset ConvertOfficeDateTimeToUniversal(DateTimeOffset officeDateTime)
    {
        return officeDateTime.ToUniversalTime();
    }
}

