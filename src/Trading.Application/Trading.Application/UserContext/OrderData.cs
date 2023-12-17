using Trading.Application.BLL.CapitalIntegrationEntities;

namespace Trading.Application.UserContext;

public class TradeData {
    public string Epic { get;set; }
    public Directions Direction { get;set; }
    public decimal? Level { get;set; }
    public decimal Size { get;set; }
    public decimal StopLoss { get;set; }
    public decimal TakeProfit { get;set; }
}