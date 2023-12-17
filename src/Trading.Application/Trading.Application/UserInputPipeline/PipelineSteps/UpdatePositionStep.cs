using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class UpdatePositionStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData == null)
        {
            return false;
        }

        var position = new CreatePositionEntity() {
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit,
            //GuaranteedStop = userContext.OrderData.StopLoss != 0 || userContext.OrderData.TakeProfit != 0
        };

        return capitalClient.UpdatePosition(userContext.InputCallback, position).Result;
    }
}