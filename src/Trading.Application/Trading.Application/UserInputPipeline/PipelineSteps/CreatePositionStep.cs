using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class CreatePositionStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input) {
        var position = new CreatePositionEntity() {
            Epic = userContext.OrderData.Epic,
            Direction = userContext.OrderData.Direction,
            Size = userContext.OrderData.Size,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit
        };

        return capitalClient.CreatePosition(position);
    }
}