namespace BookStore.Application.Abstractions.Interfaces.Persistence.Base;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
