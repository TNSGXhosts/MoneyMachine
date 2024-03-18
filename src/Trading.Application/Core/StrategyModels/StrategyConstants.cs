using Core;

namespace Trading.Application.BLL;

public static class StrategyConstants
{
    // public static readonly ICollection<string> Coins = new List<string>()
    // {
    //     BTCUSD, "XRPUSD", "SOLUSD", "SHIBUSD", "ETHUSD", "LTCUSD", "ADAUSD", "DOGEUSD", "DOTUSD", "UNIUSD",
    //     "AVAXUSD", "LINKUSD", "MATICUSD", "ALGOUSD", "XLMUSD", "TRXUSD", "BCHUSD"
    // };

    public static readonly ICollection<string> Coins = new List<string>()
    {
        BTCUSD, "ETHUSD", "BNBUSD", "SOLUSD", "XRPUSD", "ADAUSD", "DOGEUSD", "SHIBUSD",
        "AVAXUSD", "DOTUSD", "MATICUSD", "TRXUSD", "LINKUSD", "WBTCUSD", "BCHUSD", "UNIUSD", "LTCUSD",
        "ICPUSD", "FILUSD", "ETCUSD", "LEOUSD", "ATOMUSD", "IMXUSD",
        "CROUSD", "HBARUSD", "GRTUSD", "XLMUSD", "INJUSD", "PEPEUSD", "VETUSD",
        "THETAUSD", "RUNEUSD"
    };

    public const string BTCUSD = "BTCUSD";

    public static readonly Timeframe Timeframe = Timeframe.DAY;
    public static readonly Period DefaultPeriod = Period.YEAR;

    public const decimal StartBalance = 10000m;
}
