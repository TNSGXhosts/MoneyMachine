namespace Core;

using Skender.Stock.Indicators;

public class EpicTestData
{
    public required IList<Quote> AskPrices { get;set; }
    public required IList<Quote> BidPrices { get;set; }
    public required IList<SmaResult> Sma50 { get;set; }
    public required IList<SmaResult> Sma20 { get;set; }
}
