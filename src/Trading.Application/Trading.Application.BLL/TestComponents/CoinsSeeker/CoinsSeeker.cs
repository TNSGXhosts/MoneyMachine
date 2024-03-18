namespace Trading.Application.BLL;

public class CoinsSeeker(IStrategyContext strategyContext) : ICoinsSeeker
{
    public IEnumerable<string> ChooseCoinsToTrade(DateTime dateTime)
    {
        return strategyContext.Take(5).Select(i => i.Key);
    }
}
