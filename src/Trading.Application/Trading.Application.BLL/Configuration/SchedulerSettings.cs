namespace Trading.Application.BLL.Configuration;

public sealed class SchedulerSettings
{
    public TimeSpan CapitalTradingJobScheduleInterval { get; init; }

    public TimeSpan CapitalTradingHandlerJobScheduleInterval { get; init; }
}
