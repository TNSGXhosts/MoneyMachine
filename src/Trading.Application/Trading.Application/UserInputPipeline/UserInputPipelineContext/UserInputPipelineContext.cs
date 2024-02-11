using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class UserInputPipelineContext(IUserContext userContext) : IUserInputPipelineContext
{
    private bool HasPipelineError { get; set; }

    public InputPipeline? UserInputPipeline { get; set; }

    public bool ExecuteUserInputPipeline(string input)
    {
        UserInputPipeline?.ExecutePipeline(input);

        var hasPipelineError = HasPipelineError;

        HasPipelineError = false;
        UserInputPipeline = null;
        userContext.InputCallback = string.Empty;

        return hasPipelineError;
    }
}