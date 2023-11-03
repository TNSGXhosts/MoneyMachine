using Microsoft.Extensions.Logging;

namespace Trading.Application.BLL.TradingInfoCollector;

internal class CapitalTradingInfoProcessor : ICapitalTradingInfoProcessor
{
    private readonly ILogger<CapitalTradingInfoProcessor> _logger;

    public CapitalTradingInfoProcessor(ILogger<CapitalTradingInfoProcessor> logger)
    {
        _logger = logger;
    }

    public Task ProcessAsync()
    {
        // TODO : Implement
        _logger.LogInformation("CapitalTradingInfoProcessor.ProcessAsync");
        return Task.CompletedTask;
    }
}
