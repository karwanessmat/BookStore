using BookStore.Application.Abstractions.Exceptions;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Domain.ApplicationUsers.Entities;
using BookStore.Infrastructure.Outbox;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

#pragma warning disable CS8618

namespace BookStore.Infrastructure.Shared.Persistence;




public class BookStoreAppContext : IdentityDbContext<Domain.ApplicationUsers.ApplicationUser,
                                                ApplicationRole,
                                                Guid,
                                                ApplicationClaim,
                                                IdentityUserRole<Guid>,
                                                IdentityUserLogin<Guid>,
                                                ApplicationRoleClaim,
                                                IdentityUserToken<Guid>>, IUnitOfWork
{

    public DbSet<RevokedToken> RevokedTokens => Set<RevokedToken>();


    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Ignore
    };

    private readonly IDateTimeProvider _dateTimeProvider;

    public BookStoreAppContext() { }
    public BookStoreAppContext(DbContextOptions options)
        : base(options)
    {
    }
    public BookStoreAppContext(DbContextOptions options, 
                           IDateTimeProvider dateTimeProvider) : base(options)
    {
        _dateTimeProvider = dateTimeProvider;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine); // Log SQL queries to the console
        optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        Seed.ApplicationUserData(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookStoreAppContext).Assembly);


    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            AddDomainEventsAsOutboxMessages();


            int result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }


    private void AddDomainEventsAsOutboxMessages()
    {


        var outboxMessages = ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = entity.DomainEvents.ToList();

                entity.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent =>
            {
                var outBox = new OutboxMessage(
                    Guid.NewGuid(),
                    _dateTimeProvider.DefaultUtcNow,
                    domainEvent.GetType().Name,
                    JsonConvert.SerializeObject(domainEvent, JsonSerializerSettings),
                    true);

                return outBox;
            })
            .ToList();

        AddRange(outboxMessages);
    }
}
