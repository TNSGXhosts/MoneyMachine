namespace Trading.Application.BLL.CapitalIntegrationEntities;

public class Position
{
    public int ContractSize { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime CreatedDateUTC { get; set; }
    public required string DealId { get; set; }
    public required string DealReference { get; set; }
    public required string WorkingOrderId { get; set; }
    public int Size { get; set; }
    public int Leverage { get; set; }
    public double Upl { get; set; }
    public required string Direction { get; set; }
    public double Level { get; set; }
    public required string Currency { get; set; }
    public bool GuaranteedStop { get; set; }
}

public class Market
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

public class PositionData
{
    public required Position Position { get; set; }
    public required Market Market { get; set; }
}

public class PositionsResponse
{
    public required IEnumerable<PositionData> Positions { get; set; }
}