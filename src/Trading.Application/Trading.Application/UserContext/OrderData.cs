using Trading.Application.BLL.CapitalIntegrationEntities;

namespace Trading.Application.UserContext;

public class TradeData {
    public string Epic { get;set; }
    public Directions Direction { get;set; }
    public double? Level { get;set; }
    public double Size { get;set; }
    public double StopLoss { get;set; }
    public double TakeProfit { get;set; }
}