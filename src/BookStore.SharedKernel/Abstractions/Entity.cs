// ReSharper disable NonReadonlyMemberInGetHashCode

using BookStore.SharedKernel.Abstractions.IServices;

#pragma warning disable CS8618

namespace BookStore.SharedKernel.Abstractions;

public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>,IHasDomainEvents 
    where TEntityId : ValueObject
{
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public TEntityId Id { get; protected set; }

    protected Entity()
    {

    }
    protected Entity(TEntityId id)
    {
        Id = id;
    }

    #region Equality Section

    public bool Equals(Entity<TEntityId>? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TEntityId> entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity<TEntityId> left, Entity<TEntityId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TEntityId> left, Entity<TEntityId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    #endregion


    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


}
