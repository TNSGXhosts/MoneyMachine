namespace Trading.Application.UserInputPipeline;

public interface IPipelineStep
{
    bool Execute(string input);
}