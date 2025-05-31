using BookStore.SharedKernel.Abstractions;
using Mapster;

namespace BookStore.Application.Abstractions.Mapping;

public class AggregateRootIdMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AggregateRootId<Guid>, Guid>()
            .MapWith(src => src.Value);
    }
}


