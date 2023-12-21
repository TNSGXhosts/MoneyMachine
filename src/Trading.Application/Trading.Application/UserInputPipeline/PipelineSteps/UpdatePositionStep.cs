using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegration.Models;
using Trading.Application.UserContext;

// TODO : Fix namespace for all pipeline steps
namespace Trading.Application.UserInputPipeline;

public class UpdatePositionStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData is null)
        {
            return false;
        }

        var position = new CreatePositionRequestModel()
        {
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit,
        };

        return capitalClient.UpdatePositionAsync(userContext.InputCallback, position).Result;
    }
}
