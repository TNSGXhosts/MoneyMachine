namespace Trading.Application.Core.APIRequestsEntities;

// TODO : Refactor file.

public class WorkingOrderResponce
{
    public required IEnumerable<WorkingOrder> WorkingOrders { get; set; }
}

public class WorkingOrder
{
    public required WorkingOrderData WorkingOrderData { get; set; }
    public required MarketData MarketData { get; set; }
}

public class WorkingOrderData
{
    public required string DealId { get; set; }
    public required string Direction { get; set; }
    public required string Epic { get; set; }
    public double OrderSize { get; set; }
    public double OrderLevel { get; set; }
    public required string TimeInForce { get; set; }
    public DateTime GoodTillDate { get; set; }
    public DateTime GoodTillDateUTC { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime CreatedDateUTC { get; set; }
    public bool GuaranteedStop { get; set; }
    public required string OrderType { get; set; }
    public double StopDistance { get; set; }
    public double ProfitDistance { get; set; }
    public string CurrencyCode { get; set; }
}

public class MarketData
{
    public required string InstrumentName { get; set; }
    public required string Expiry { get; set; }
    public required string MarketStatus { get; set; }
    public required string Epic { get; set; }
    public required string InstrumentType { get; set; }
    public int LotSize { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double PercentageChange { get; set; }
    public double NetChange { get; set; }
    public double Bid { get; set; }
    public double Offer { get; set; }
    public DateTime UpdateTime { get; set; }
    public DateTime UpdateTimeUTC { get; set; }
    public int DelayTime { get; set; }
    public bool StreamingPricesAvailable { get; set; }
    public int ScalingFactor { get; set; }
}