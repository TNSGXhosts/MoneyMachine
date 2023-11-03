using Microsoft.Extensions.Logging;

using Quartz;

using Trading.Application.BLL.TradingHandler;

namespace Trading.Application.BLL.Background.Jobs;

[DisallowConcurrentExecution]
internal class CapitalTradingHandlerJob : JobBase
{
    public CapitalTradingHandlerJob(ILogger<CapitalTradingHandlerJob> logger, ICapitalTradingHandlerProcessor processor)
        : base(logger, processor)
    {
    }
}
