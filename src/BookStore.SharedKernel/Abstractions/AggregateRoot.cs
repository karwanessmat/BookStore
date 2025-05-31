namespace BookStore.SharedKernel.Abstractions;
/// <summary>
/// 
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TEntityId"></typeparam>
    public abstract class AggregateRoot<TEntity,TEntityId>:Entity<TEntity> where TEntity : AggregateRootId<TEntityId>
    {
        public new AggregateRootId<TEntityId> Id { get; protected set; }

        protected AggregateRoot(TEntity id) : base(id)
        {
            Id = id;
        }

        protected AggregateRoot()
        {

        }
    }