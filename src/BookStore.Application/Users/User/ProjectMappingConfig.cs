using BookStore.Application.Users.User.Commands.Register;
using BookStore.Contracts.Users;
using Mapster;

namespace BookStore.Application.Users.User;
public class ProjectMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>()
            .Map(disc => disc.Request, src => src)  
            .IgnoreNullValues(true);
    }
}
