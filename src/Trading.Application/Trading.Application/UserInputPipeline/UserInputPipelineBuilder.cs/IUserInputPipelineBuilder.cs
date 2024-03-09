namespace Trading.Application.UserInputPipeline;

public interface IUserInputPipelineBuilder
{
    void BuildAddPositionPipeline();
    void BuildEditPositionPipeline();
    void BuildAddOrderPipeline();
    void BuildEditOrderPipeline();
}