using BookStore.Infrastructure.Shared.Persistence;
using BookStore.SharedKernel.Abstractions.IServices;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace BookStore.Infrastructure.Outbox;

[DisallowConcurrentExecution] // it is going to make sure that there is only one outbox messages jobs instance running at any given time
internal sealed class ProcessOutboxMessagesJob(BookStoreAppContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IPublisher publisher,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger) : IJob
{
    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages");

        dbContext.Database.SetCommandTimeout(120);

        await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();
        List<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync();

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;
                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {


                logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            

            await UpdateOutboxMessageAsync(outboxMessage, exception);
        }

        await transaction.CommitAsync();

      
        const string deleteCommand = "delete from outbox_messages where ProcessedOnUtc is not null and IsActive = 0"; ;
       
        await dbContext.Database.ExecuteSqlRawAsync(deleteCommand);
        logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<List<OutboxMessageResponse>> GetOutboxMessagesAsync()
    {
        try
        {
            string? sql = $"""
                           SELECT TOP ({_outboxOptions.BatchSize}) Id, Content
                           FROM outbox_messages
                           WHERE ProcessedOnUtc IS NULL
                           ORDER BY OccurredOnUtc
                           """;

            List<OutboxMessageResponse>? outboxMessages = await dbContext
                .Database
                .SqlQueryRaw<OutboxMessageResponse>(sql)
                .AsNoTracking()
                .ToListAsync();
            return outboxMessages!;
        }
        catch (Exception e)
        {
        }

        return new List<OutboxMessageResponse>();

    }


    private async Task UpdateOutboxMessageAsync(OutboxMessageResponse outboxMessage, Exception? exception)
    {
        string errorInfo = exception?.ToString() ?? string.Empty;
        // Truncate error info if necessary to fit into your database schema constraints

        await dbContext.Database.ExecuteSqlRawAsync("""
            UPDATE outbox_messages
            SET ProcessedOnUtc = {0}, error = {1}, IsActive = {2}
            WHERE id = {3}
            """,
            dateTimeProvider.DefaultUtcNow, errorInfo,false, outboxMessage.Id);
    }

    internal sealed record OutboxMessageResponse()
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
    };
}