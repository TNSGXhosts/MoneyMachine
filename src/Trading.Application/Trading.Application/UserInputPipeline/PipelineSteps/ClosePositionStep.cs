using Trading.Application.BLL.CapitalIntegration;

namespace Trading.Application.UserInputPipeline;

public class ClosePositionStep(ICapitalClient capitalClient) : IPipelineStep
{
    public bool Execute(string input)
    {
        return capitalClient.ClosePosition(input);
    }
}