using NodaTime;

namespace BookStore.Infrastructure.Abstractions.Helpers
{
    public static class DateTimeHelper
    {

        public static Task<DateTimeOffset> GetCurrentDateTimeOffsetByTimeZone(string? timeZoneId = null)
        {
            const string defaultTimeZoneId = "Asia/Baghdad";

            try
            {
                timeZoneId ??= defaultTimeZoneId;
                return Task.FromResult(GetCurrentDateTimeOffsetForZone(timeZoneId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetCurrentDateTimeOffsetByTimeZone: {ex.Message}");
                throw;
            }
        }

        private static DateTimeOffset GetCurrentDateTimeOffsetForZone(string timeZoneId)
        {
            var timeZone = DateTimeZoneProviders.Tzdb[timeZoneId];
            var currentInstant = SystemClock.Instance.GetCurrentInstant();
            var time = currentInstant.InZone(timeZone);
            return time.ToDateTimeOffset();
        }
    }
}
