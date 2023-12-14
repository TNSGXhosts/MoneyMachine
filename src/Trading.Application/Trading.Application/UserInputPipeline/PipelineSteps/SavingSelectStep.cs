using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class SavingSelectStep(IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input) {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        userContext.InputCallback = input;
        return true;
    }
}