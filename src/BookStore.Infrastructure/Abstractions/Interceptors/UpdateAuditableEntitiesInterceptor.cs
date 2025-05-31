using BookStore.Application.Abstractions.Authentication;
using BookStore.Infrastructure.Abstractions.Helpers;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookStore.Infrastructure.Abstractions.Interceptors;

public class UpdateAuditableEntitiesInterceptor(ICurrentUserProvider currentUserProvider) : SaveChangesInterceptor
{





    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {


        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        UpdateEntities(eventData.Context!);
        var saveResult = base.SavingChangesAsync(eventData, result, cancellationToken);
        return saveResult;
    }

    private void UpdateEntities(DbContext context)
    {



        var baghdadTime = DateTimeHelper.GetCurrentDateTimeOffsetByTimeZone().GetAwaiter().GetResult();


        var entries = context.ChangeTracker.Entries<IAuditableEntity>();
        
        foreach (var entry in entries)
        {
            var entity = entry.Entity;
            var entityType = entity.GetType();

            switch (entry.State)
            {
                case EntityState.Added:
                    entityType.GetProperty("CreatedBy")?.SetValue(entity, currentUserProvider.GetUserId == Guid.Empty ? null : currentUserProvider.GetUserId);
                    entityType.GetProperty("CreatedDateTimeOnUtc")?.SetValue(entity, baghdadTime);
                    entityType.GetProperty("UpdatedDateTimeOnUtc")?.SetValue(entity, baghdadTime);
                    break;

                case EntityState.Modified:
                    entityType.GetProperty("LastModifiedBy")?.SetValue(entity, currentUserProvider.GetUserId == Guid.Empty ? null : currentUserProvider.GetUserId);
                    entityType.GetProperty("UpdatedDateTimeOnUtc")?.SetValue(entity, baghdadTime);
                    break;
            }
        }


    }
}