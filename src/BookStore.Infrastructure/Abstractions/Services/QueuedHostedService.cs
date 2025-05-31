using BookStore.Application.Abstractions.Interfaces.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BookStore.Infrastructure.Abstractions.Services;

public class QueuedHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueuedHostedService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Queued Hosted Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await taskQueue.DequeueAsync(stoppingToken);

            try
            {
                await workItem?.Invoke(stoppingToken)!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred executing work item.");
            }
        }

        logger.LogInformation("Queued Hosted Service is stopping.");
    }
}