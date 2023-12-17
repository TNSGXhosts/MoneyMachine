using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegrationEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class UpdateOrderStep(ICapitalClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData == null || !userContext.OrderData.Level.HasValue)
        {
            return false;
        }

        var position = new UpdateOrderEntity() {
            Level = (decimal)userContext.OrderData.Level,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit,
            //GuaranteedStop = userContext.OrderData.StopLoss != 0 || userContext.OrderData.TakeProfit != 0
        };

        return capitalClient.UpdateOrder(userContext.InputCallback, position).Result;
    }
}