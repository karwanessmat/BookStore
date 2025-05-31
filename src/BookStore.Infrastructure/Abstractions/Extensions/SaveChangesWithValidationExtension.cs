using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using BookStore.Infrastructure.Shared.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#pragma warning disable CS8603

namespace BookStore.Infrastructure.Abstractions.Extensions;

public static class SaveChangesWithValidationExtension
{

    public static async Task<ImmutableList<ValidationResult>> SaveChangesWithValidation(this BookStoreAppContext dbContext)
    {
        ImmutableList<ValidationResult>? result = dbContext.ExecuteValidation();
        if (result.Any())
        {
            return result;
        }

        await dbContext.SaveChangesAsync();

        return result;
    }


    private static ImmutableList<ValidationResult> ExecuteValidation(this BookStoreAppContext dbContext)
    {
        var result = new List<ValidationResult>();
        IEnumerable<EntityEntry>? entries = dbContext
            .ChangeTracker
            .Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);


        foreach (EntityEntry? entry in entries)
        {
            object? entity = entry.Entity;
            var valProvider = new ValidationDbContextServiceProvider(dbContext);
            var valContext = new ValidationContext(entity, valProvider, null);
            var entityErrors = new List<ValidationResult>();
            if (!Validator.TryValidateObject(entity, valContext, entityErrors, true))
            {
                result.AddRange(entityErrors);
            }
        }

        return result.ToImmutableList();
    }
}

public class ValidationDbContextServiceProvider : IServiceProvider
{
    private readonly BookStoreAppContext _dbContext;

    public ValidationDbContextServiceProvider(BookStoreAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public object GetService(Type serviceType)
    {
        return serviceType == typeof(DbContext) ? _dbContext : null;
    }
}
