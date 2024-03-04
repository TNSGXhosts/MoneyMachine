using Microsoft.Extensions.Logging;

using Trading.Application.BLL;

using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.UserContext;

namespace Trading.Application.UserInputPipeline;

public class UserInputPipelineBuilder(IUserContext userContext,
                                    IUserInputPipelineContext userInputPipelineContext,
                                    ILogger<UserInputPipelineBuilder> logger,
                                    IOrderClient orderClient,
                                    IPositionClient positionClient,
                                    IStateProcessor stateProcessor,
                                    IPriceRepository priceRepository,
                                    IDataManager dataManager) : IUserInputPipelineBuilder
{
    public void BuildAddOrderPipeline()
    {
        userInputPipelineContext.UserInputPipeline = new InputPipeline(new List<IPipelineStep>() {
                new ParseTradeCreationStep(userContext, logger, true),
                new CreateOrderStep(orderClient, userContext),
            }, userContext);
    }

    public void BuildAddPositionPipeline()
    {
        userInputPipelineContext.UserInputPipeline = new InputPipeline(new List<IPipelineStep>(){
                new ParseTradeCreationStep(userContext, logger, false),
                new CreatePositionStep(positionClient, userContext),
            }, userContext);
    }

    public void BuildEditOrderPipeline()
    {
        userInputPipelineContext.UserInputPipeline = new InputPipeline(new List<IPipelineStep>(){
                new ParseTradeUpdateStep(userContext, logger, true),
                new UpdateOrderStep(orderClient, userContext),
            }, userContext);
    }

    public void BuildEditPositionPipeline()
    {
        userInputPipelineContext.UserInputPipeline = new InputPipeline(new List<IPipelineStep>(){
                new ParseTradeUpdateStep(userContext, logger, false),
                new UpdatePositionStep(positionClient, userContext),
            }, userContext);
    }

    public void BuildTestStrategyPipeline()
    {
        userInputPipelineContext.UserInputPipeline = new InputPipeline(new List<IPipelineStep>(){
                new ParseTestStrategyStep(userContext),
                new RunStrategyTestStep(userContext, priceRepository, stateProcessor, dataManager)
            }, userContext);
    }
}