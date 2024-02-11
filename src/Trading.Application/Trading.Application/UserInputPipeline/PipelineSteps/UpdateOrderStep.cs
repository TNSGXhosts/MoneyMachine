using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegration.Models;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class UpdateOrderStep(IOrderClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData == null || !userContext.OrderData.Level.HasValue)
        {
            return false;
        }

        var position = new UpdateOrderRequestModel
        {
            Level = (decimal)userContext.OrderData.Level,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit,
        };

        return capitalClient.UpdateOrderAsync(userContext.InputCallback, position).Result;
    }
}
