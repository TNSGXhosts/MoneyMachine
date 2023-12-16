using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class CreateOrderStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input) {
        var order = new CreateOrderEntity() {
            Epic = userContext.OrderData.Epic,
            Direction = userContext.OrderData.Direction,
            Size = (double)userContext.OrderData.Level,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit
        };

        return capitalClient.CreateOrder(order);
    }
}