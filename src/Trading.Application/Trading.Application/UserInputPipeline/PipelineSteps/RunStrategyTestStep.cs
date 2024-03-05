using Core;

using Trading.Application.BLL;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class RunStrategyTestStep(
    IUserContext userContext,
    IPriceRepository priceRepository,
    IDataManager dataManager) : IPipelineStep
{
    public bool Execute(string input)
    {
        dataManager.DownloadAndSavePrices(userContext.OrderData.Epic, Timeframe.DAY);

        var strategy = new StrategyProcessor(input, priceRepository);
        strategy.Run();

        return true;
    }
}
