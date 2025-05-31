#pragma warning disable CS8618

namespace BookStore.SharedKernel.Abstractions;

public abstract class AggregateRootId<TEntityId> : ValueObject
{
    public  abstract TEntityId Value { get; protected set; } 
}