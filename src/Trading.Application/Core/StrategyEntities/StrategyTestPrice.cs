namespace Core;

using Skender.Stock.Indicators;

public class EpicTestData
{
    public required Quote AskPrice { get;set; }
    public required Quote BidPrice { get;set; }
    public required SmaResult Sma50 { get;set; }
    public required SmaResult Sma20 { get;set; }
}
