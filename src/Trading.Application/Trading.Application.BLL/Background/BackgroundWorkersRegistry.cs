using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Quartz;

using Trading.Application.BLL.Background.Jobs;
using Trading.Application.BLL.Configuration;

namespace Trading.Application.BLL.Background;

public static class BackgroundWorkersRegistry
{
    public static void RegisterBackgroundWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        var schedulerSettings = new SchedulerSettings();
        configuration.GetSection(nameof(SchedulerSettings)).Bind(schedulerSettings);

        services.AddQuartz(q
            => q.Configure<CapitalTradingInfoCollectorJob>(schedulerSettings.CapitalTradingJobScheduleInterval));

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    }

    private static void Configure<T>(this IServiceCollectionQuartzConfigurator q,
        TimeSpan scheduleInterval)
        where T : IJob
    {
        q.ScheduleJob<T>(trigger => trigger
            .WithIdentity(typeof(T).Name)
            .StartNow()
            .WithSimpleSchedule(builder => builder.WithInterval(scheduleInterval).RepeatForever())
            .WithDescription(string.Empty));
    }
}
