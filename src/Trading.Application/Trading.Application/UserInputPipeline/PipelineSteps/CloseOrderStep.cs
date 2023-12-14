using Trading.Application.BLL.CapitalIntegration;

namespace Trading.Application.UserInputPipeline;

public class CloseOrderStep(ICapitalClient capitalClient) : IPipelineStep
{
    public bool Execute(string input)
    {
        return capitalClient.CloseOrder(input);
    }
}