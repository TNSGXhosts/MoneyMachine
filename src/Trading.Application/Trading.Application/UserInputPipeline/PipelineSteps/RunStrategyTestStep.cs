using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class RunStrategyTestStep(IUserContext userContext, IStateProcessor stateProcessor) : IPipelineStep
{
    public bool Execute(string input)
    {
        stateProcessor.CatchEvent(TelegramConstants.Triggers.Start);

        return true;
    }
}
