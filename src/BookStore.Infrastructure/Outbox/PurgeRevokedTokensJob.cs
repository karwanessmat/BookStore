using BookStore.Infrastructure.Abstractions.Authentication;
using BookStore.Infrastructure.Shared.Persistence;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace BookStore.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class PurgeRevokedTokensJob(
    BookStoreAppContext db,
    IOptions<RevokedTokenOptions> opt, 
    IDateTimeProvider clock,
    ILogger<PurgeRevokedTokensJob> log) : IJob
{
    private readonly RevokedTokenOptions _opt = opt.Value;

    public async Task Execute(IJobExecutionContext ctx)
    {
        // because exp date is in UTC, we use the clock's DefaultUtcNow to utcNow
        var utcNow = clock.DefaultUtcNow.UtcDateTime;

        var totalRemoved = 0;
        while (!ctx.CancellationToken.IsCancellationRequested)
        {
            var removed = await db.Database.ExecuteSqlInterpolatedAsync($"""
                                                                        DELETE TOP ({_opt.BatchSize})
                                                                        FROM   Security.RevokedTokens
                                                                        WHERE  ExpiryUtc < {utcNow}
                                                                        """, ctx.CancellationToken);

            totalRemoved += removed;
            if (removed < _opt.BatchSize)
                break;
        }
    }
}