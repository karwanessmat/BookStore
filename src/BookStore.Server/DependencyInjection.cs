using Serilog;

namespace BookStore.Server;


public static class DependencyInjection
{
    public static IServiceCollection AddServer(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

        return services;
    }
}
