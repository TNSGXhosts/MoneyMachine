using Microsoft.Extensions.Logging;

using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeUpdateStep(IUserContext userContext, ILogger<ParseTradeUpdateStep> logger) : IPipelineStep
{
    public bool Execute(string input)
    {
        string[] lines = input.Split('\n');

        try{
            var isOrder = lines.Length == 3;
            userContext.OrderData = new TradeData
            {
                Level = isOrder ? double.Parse(lines[0]) : null,
                StopLoss = double.Parse(lines[isOrder ? 1 : 0]),
                TakeProfit = double.Parse(lines[isOrder ? 2 : 1])
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