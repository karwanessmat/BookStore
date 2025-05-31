using BookStore.SharedKernel.Abstractions;

namespace BookStore.Domain.ApplicationUsers.ValueObjects;

public class RevokedTokenId : ValueObject
{
    private RevokedTokenId() { }
    public Guid Value { get; private set; }
    private RevokedTokenId(Guid value)
    {
        Value = value;
    }

    public static RevokedTokenId CreateUnique()
    {
        return new RevokedTokenId(Guid.NewGuid());
    }

    public static RevokedTokenId Create(Guid value)
    {
        return new RevokedTokenId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

}