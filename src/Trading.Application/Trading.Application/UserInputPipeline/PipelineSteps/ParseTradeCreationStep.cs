using Microsoft.Extensions.Logging;

using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeCreationStep(IUserContext userContext, ILogger<ParseTradeCreationStep> logger) : IPipelineStep
{
    public bool Execute(string input)
    {
        string[] lines = input.Split('\n');
        var isOrder = lines.Length == 6;

        try {
            userContext.OrderData = new TradeData
            {
                Epic = lines[0],
                Direction = (Directions)Enum.Parse(typeof(Directions), lines[1]),
                Size = double.Parse(lines[2]),
                Level = isOrder ? double.Parse(lines[3]) : null,
                StopLoss = double.Parse(lines[isOrder ? 4 : 3]),
                TakeProfit = double.Parse(lines[isOrder ? 5 : 4]),
            };
        } catch {
            logger.LogError($"Can't parse input: {input}");

            return false;
        }

        return true;
    }
}