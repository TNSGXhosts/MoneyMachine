using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class ParseTestStrategyStep(IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        userContext.OrderData = new TradeData() { Epic = input.Trim() };

        return true;
    }
}
