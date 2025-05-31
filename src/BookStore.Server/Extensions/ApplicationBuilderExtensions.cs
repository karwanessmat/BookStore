using BookStore.Infrastructure.Shared.Persistence;
using BookStore.Server.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Server.Extensions;

public  static partial class ApplicationBuilderExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BookStoreAppContext>();

            if (dbContext.Database.CanConnect())
            {
             // dbContext.Database.EnsureDeleted();
            }

            dbContext.Database.Migrate();
        }
        catch (Exception a)
        {
            Console.WriteLine(a);
        }

    }
}

public static partial class ApplicationBuilderExtensions
{
    public static void UseCustomMiddleware(this IApplicationBuilder app)
    {
        UseCustomExceptionHandler(app);
        UseRequestContextLogging(app);
        UseRequestTimingMiddleware(app);
    }


    public static void UseCustomExceptionHandler(IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }


    public static void UseRequestContextLogging(IApplicationBuilder app)
    {
        app.UseMiddleware<RequestContextLoggingMiddleware>();
    }

    public static void UseJwtDiagnosticMiddleware(IApplicationBuilder app)
    {
        app.UseMiddleware<JwtDiagnosticMiddleware>();
    }

    public static void UseRequestTimingMiddleware(IApplicationBuilder app)
    {
        app.UseMiddleware<RequestTimingMiddleware>();
    }
}