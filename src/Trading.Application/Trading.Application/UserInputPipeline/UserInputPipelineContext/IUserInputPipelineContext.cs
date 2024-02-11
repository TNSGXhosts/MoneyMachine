namespace Trading.Application.UserInputPipeline;

public interface IUserInputPipelineContext
{
    InputPipeline UserInputPipeline { get; set; }

    bool ExecuteUserInputPipeline(string input);
}