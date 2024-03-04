using Core;

using Trading.Application.BLL;
using Trading.Application.UserContext;
using Trading.Application.UserInputPipeline;

namespace Trading.Application;

public class RunStrategyTestStep(
    IUserContext userContext,
    IPriceRepository priceRepository,
    IStateProcessor stateProcessor,
    IDataManager dataManager) : IPipelineStep
{
    public bool Execute(string input)
    {
        dataManager.DownloadAndSavePrices(userContext.OrderData.Epic, nameof(Timeframe.DAY));

        var strategy = new StrategyProcessor(input, priceRepository);
        strategy.Run();

        stateProcessor.CatchEvent(TelegramConstants.Triggers.Start);

        return true;
    }
}
