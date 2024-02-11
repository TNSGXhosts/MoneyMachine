using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class InputPipeline
{
    private readonly IEnumerable<IPipelineStep> _pipelineSteps;
    private readonly IUserContext _userContext;

    public InputPipeline(IEnumerable<IPipelineStep> pipelineSteps, IUserContext userContext)
    {
        _pipelineSteps = pipelineSteps;
        _userContext = userContext;
    }

    public void ExecutePipeline(string input)
    {
        foreach (var step in _pipelineSteps)
        {
            if (!step.Execute(input))
            {
                _userContext.HasPipelineError = true;
            }
        }
    }
}