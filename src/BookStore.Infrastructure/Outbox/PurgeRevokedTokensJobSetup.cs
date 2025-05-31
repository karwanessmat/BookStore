using BookStore.Infrastructure.Abstractions.Authentication;
using Microsoft.Extensions.Options;
using Quartz;

namespace BookStore.Infrastructure.Outbox;

internal sealed class PurgeRevokedTokensJobSetup(
    IOptions<RevokedTokenOptions> opt)
    : IConfigureOptions<QuartzOptions>
{
    private readonly RevokedTokenOptions _opt = opt.Value;

    public void Configure(QuartzOptions options)
    {
        const string jobKey = nameof(PurgeRevokedTokensJob);

        options.AddJob<PurgeRevokedTokensJob>(cfg => cfg.WithIdentity(jobKey))
            .AddTrigger(cfg => cfg
                .ForJob(jobKey)
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(_opt.PurgeIntervalMinutes)
                    .RepeatForever()));
    }
}