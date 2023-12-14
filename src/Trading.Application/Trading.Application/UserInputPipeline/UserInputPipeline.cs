using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class InputPipeline
{
    public List<IPipelineStep> PipelineSteps;
    public IUserContext UserContext;

    public void ExecutePipeline(string input) {
        foreach(var step in PipelineSteps) {
            if (!step.Execute(input)) {
                UserContext.HasPipelineError = true;
            }
        }
    }
}