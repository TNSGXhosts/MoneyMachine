using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.BLL.CapitalIntegration.Models;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class CreatePositionStep(IPositionClient positionClient, IUserContext userContext) : IPipelineStep
{
    public bool Execute(string input)
    {
        if (userContext.OrderData == null)
        {
            return false;
        }

        var position = new CreatePositionRequestModel
        {
            Epic = userContext.OrderData.Epic,
            Direction = userContext.OrderData.Direction.ToString(),
            Size = userContext.OrderData.Size,
            StopLevel = userContext.OrderData.StopLoss,
            ProfitLevel = userContext.OrderData.TakeProfit
        };

        return positionClient.CreatePositionAsync(position).Result;
    }
}
