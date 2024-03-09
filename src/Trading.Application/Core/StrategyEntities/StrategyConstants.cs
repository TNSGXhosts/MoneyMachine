namespace Trading.Application.BLL;

public static class StrategyConstants
{
    public static readonly ICollection<string> Coins = new List<string>()
    {
        BTCUSD, "XRPUSD", "SOLUSD", "SHIBUSD", "ETHUSD", "LTCUSD", "ADAUSD", "DOGEUSD", "DOTUSD", "UNIUSD",
        "AVAXUSD", "LINKUSD", "MATICUSD", "ALGOUSD", "XLMUSD", "TRXUSD", "BCHUSD"
    };

    public const string BTCUSD = "BTCUSD";
}
