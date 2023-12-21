using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

// TODO : Pipeline should be implemented in other way. This is just a quick and dirty solution.
// Creation of pipeline should be done in a different way like:
// 1. Create a pipeline builder
// 2. Add steps to the pipeline builder
// 3. Build the pipeline
// 4. Add pipeline to the user context
// 5. Execute the pipeline when needed
// It should manage own dynamic pipeline context for every pipeline.

public class InputPipeline
{
    public List<IPipelineStep> PipelineSteps;
    public IUserContext UserContext;

    public void ExecutePipeline(string input)
    {
        foreach (var step in PipelineSteps)
        {
            if (!step.Execute(input))
            {
                UserContext.HasPipelineError = true;
            }
        }
    }
}