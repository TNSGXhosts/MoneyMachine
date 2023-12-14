using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeUpdateStep(IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        //TODO: validate input
        string[] lines = input.Split('\n');

        if (lines.Length == 3 || lines.Length == 4)
        {
            return false;
        }

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
            return false;
        }

        return true;
    }
}