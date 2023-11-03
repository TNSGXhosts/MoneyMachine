using Microsoft.Extensions.Logging;

using Quartz;

using Trading.Application.BLL.TradingInfoCollector;

namespace Trading.Application.BLL.Background.Jobs;

[DisallowConcurrentExecution]
internal class CapitalTradingInfoCollectorJob : JobBase
{
    public CapitalTradingInfoCollectorJob(ILogger<CapitalTradingInfoCollectorJob> logger, ICapitalTradingInfoProcessor processor)
        : base(logger, processor)
    {
    }
}
