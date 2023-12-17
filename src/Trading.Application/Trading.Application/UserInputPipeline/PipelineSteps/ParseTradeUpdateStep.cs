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
            var isOrder = lines.Length == 3;
            userContext.OrderData = new TradeData
            {
                Level = isOrder ? double.Parse(lines[0]) : null,
                StopLoss = hasStoploss ? double.Parse(lines[isOrder ? 1 : 0]) : 0,
                TakeProfit = hasTakeprofit ? double.Parse(lines[isOrder ? 2 : 1]) : 0
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