using BookStore.Application.Abstractions.Interfaces.Persistence.Base;

namespace BookStore.Infrastructure.Shared.Persistence.Repositories.Base;

public class RepositoryManager(BookStoreAppContext spardaDbContext) : IRepositoryManager
{
    // Lazy initialization for repositories
    //private readonly Lazy<IContractorRepository> _contractorRepository = new(() => new ContractorRepository(spardaDbContext));

    // Expose repositories via properties
    //public IContractorRepository Contractors => _contractorRepository.Value;


    public async ValueTask DisposeAsync()
    {
        await spardaDbContext.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}