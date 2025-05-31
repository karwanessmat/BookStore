using BookStore.SharedKernel.Abstractions.Errors;
using Throw;

namespace BookStore.SharedKernel.Abstractions.ValueObjects;

public class TimeRange(TimeOnly start, TimeOnly end) : ValueObject
{
    public TimeOnly Start { get; init; } = start.Throw().IfGreaterThanOrEqualTo(end);
    public TimeOnly End { get; init; } = end;

    public static Result<TimeRange> FromDateTimes(DateTimeOffset start, DateTimeOffset end)
    {
        if (start.Date != end.Date)
        {
            return Result.Failure<TimeRange>(Error.Validation(description: "Start and end date times must be on the same day."));
        }

        if (start >= end)
        {
            return Result.Failure<TimeRange>(Error.Validation(description: "End time must be greater than the start time"));
        }

        return new TimeRange(
            start: TimeOnly.FromDateTime(start.UtcDateTime),
            end: TimeOnly.FromDateTime(end.UtcDateTime));
    }

    public bool OverlapsWith(TimeRange other)
    {
        if (Start >= other.End) return false;
        
        return other.Start < End;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}