
namespace BookStore.SharedKernel.Abstractions.ValueObjects;

public class DateRange:ValueObject
{
    public DateOnly ActiveDate { get; init; }
    public DateOnly ExpiredDate { get; init; }

    public int LengthInDays => ExpiredDate.DayNumber - ActiveDate.DayNumber;

    public static DateRange Create(DateOnly activeDate, DateOnly expiredDate)
    {
        if (activeDate > expiredDate)
        {
            throw new ApplicationException("expired date date precedes active date");
        }

        return new DateRange()
        {
            ActiveDate = activeDate,
            ExpiredDate = expiredDate
        };
    }

    public static DateRange Create(DateOnly activeDate,int subscriptionPeriod )
    {
        DateOnly expiredDate = subscriptionPeriod switch
        {
            1 => activeDate.AddMonths(1),
            2 => activeDate.AddMonths(3),
            6 => activeDate.AddMonths(6),
            12 => activeDate.AddYears(1),
            18 => activeDate.AddYears(1).AddMonths(6),
            24 => activeDate.AddYears(2),
            _ => new DateOnly()
        };

        return new DateRange()
        {
            ActiveDate = activeDate,
            ExpiredDate = expiredDate
        };
    }


    public bool OverlapsWith(DateRange other)
    {
        if (ActiveDate >= other.ExpiredDate) return false;

        return other.ActiveDate < ExpiredDate;
    }



    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return ActiveDate;
        yield return ExpiredDate;
    }
}
