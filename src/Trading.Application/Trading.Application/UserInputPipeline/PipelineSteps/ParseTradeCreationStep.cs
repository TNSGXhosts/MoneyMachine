using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class ParseTradeCreationStep(IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        string[] lines = input.Split('\n');
        var isOrder = lines.Length == 6;
        Directions direction;
        if (!Enum.TryParse(lines[1], true, out direction) && (lines.Length == 6 || lines.Length == 5)) {
            return false;
        }

        try {
            userContext.OrderData = new TradeData
            {
                Epic = lines[0],
                Direction = direction,
                Size = double.Parse(lines[2]),
                Level = isOrder ? double.Parse(lines[3]) : null,
                StopLoss = double.Parse(lines[isOrder ? 4 : 3]),
                TakeProfit = double.Parse(lines[isOrder ? 5 : 4]),
            };
        } catch {
            return false;
        }

        return true;
    }
}