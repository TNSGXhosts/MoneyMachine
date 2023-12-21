using Trading.Application.BLL.CapitalIntegration.Enums;

namespace Trading.Application.UserContext;

// Why is it here? Do something with it
public class TradeData {
    public string Epic { get;set; }
    public Directions Direction { get;set; }
    public decimal? Level { get;set; }
    public decimal? Size { get;set; }
    public decimal? StopLoss { get;set; }
    public decimal? TakeProfit { get;set; }
}