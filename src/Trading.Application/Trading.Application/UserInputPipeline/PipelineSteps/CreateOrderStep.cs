using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.Core.APIRequestsEntities;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class CreateOrderStep(IOrderClient capitalClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData == null || !userContext.OrderData.Level.HasValue)
        {
            return false;
        }

        var order = new CreateOrderRequestModel
        {
            Epic = userContext.OrderData.Epic,
            Direction = userContext.OrderData.Direction.ToString(),
            Size = userContext.OrderData.Size,
            Level = userContext.OrderData.Level,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit,
            Type = Types.LIMIT
        };

        return capitalClient.CreateOrderAsync(order).Result;
    }
}
