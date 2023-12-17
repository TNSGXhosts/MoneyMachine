using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class CreatePositionStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input) {
        if (userContext.OrderData == null)
        {
            return false;
        }

        var position = new CreatePositionEntity() {
            Epic = userContext.OrderData.Epic,
            Direction = userContext.OrderData.Direction.ToString(),
            Size = userContext.OrderData.Size,
            //GuaranteedStop = userContext.OrderData.StopLoss != 0 || userContext.OrderData.TakeProfit != 0,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit
        };

        return capitalClient.CreatePosition(position).Result;
    }
}