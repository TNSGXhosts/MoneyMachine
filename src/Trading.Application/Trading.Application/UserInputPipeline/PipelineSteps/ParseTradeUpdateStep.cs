using Microsoft.Extensions.Logging;

using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeUpdateStep(IUserContext userContext, ILogger<ParseTradeUpdateStep> logger, bool isOrder) : IPipelineStep
{
    public bool Execute(string input)
    {
        string[] lines = input.Split('\n');

        var hasStoploss = (isOrder && lines.Length >= 2) || (!isOrder && lines.Length >= 1);
        var hasTakeprofit = (isOrder && lines.Length >= 3) || (!isOrder && lines.Length >= 2);

        try {
            userContext.OrderData = new TradeData
            {
                Level = isOrder ? decimal.Parse(lines[0]) : null,
                StopLoss = hasStoploss ? decimal.Parse(lines[isOrder ? 1 : 0]) : null,
                TakeProfit = hasTakeprofit ? decimal.Parse(lines[isOrder ? 2 : 1]) : null
            };
        }
        catch
        {
            logger.LogError($"Can't parse input: {input}");

            return false;
        }

        return true;
    }
}