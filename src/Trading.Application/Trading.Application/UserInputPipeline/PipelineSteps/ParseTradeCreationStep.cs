using Microsoft.Extensions.Logging;

using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeCreationStep(IUserContext userContext, ILogger<ParseTradeCreationStep> logger, bool isOrder) : IPipelineStep
{
    public bool Execute(string input)
    {
        string[] lines = input.Split('\n');

        var hasStoploss = (isOrder && lines.Length >= 5) || (!isOrder && lines.Length >= 4);
        var hasTakeprofit = (isOrder && lines.Length >= 6) || (!isOrder && lines.Length >= 5);

        try {
            userContext.OrderData = new TradeData
            {
                Epic = lines[0],
                Direction = (Directions)Enum.Parse(typeof(Directions), lines[1]),
                Size = double.Parse(lines[2]),
                Level = isOrder ? double.Parse(lines[3]) : null,
                StopLoss = hasStoploss ? double.Parse(lines[isOrder ? 4 : 3]) : 0,
                TakeProfit = hasTakeprofit ? double.Parse(lines[isOrder ? 5 : 4]) : 0,
            };
        } catch {
            logger.LogError($"Can't parse input: {input}");

            return false;
        }

        return true;
    }
}