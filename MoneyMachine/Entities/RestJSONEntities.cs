namespace MoneyMachine.Entities
{
    public class AccountsEntity
    {
        public List<Account> Accounts { get; set; }
    }

    public class Account
    {
        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }
        public string AccountType { get; set; }
        public bool Preferred { get; set; }
        public BalanceEntity Balance { get; set; }
        public string Currency { get; set; }
    }

    public class BalanceEntity
    {
        public double Balance { get; set; }
        public double Deposit { get; set; }
        public double ProfitLoss { get; set; }
        public double Available { get; set; }
    }

    public class SessionConnectData
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
    }

    public class SessionInfo
    {
        public string ClientId { get; set; }
        public string AccountId { get; set; }
        public int TimeZone { get; set; }
        public string Locale { get; set; }
        public string Currency { get; set; }
        public string StreamEndpoint { get; set; }
    }

    public class StatusEntity
    {
        public string Status { get; set; }
    }

    public class Position
    {
        public int ContractSize { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateUTC { get; set; }
        public string DealId { get; set; }
        public string DealReference { get; set; }
        public string WorkingOrderId { get; set; }
        public double Size { get; set; }
        public int Leverage { get; set; }
        public double Upl { get; set; }
        public string Direction { get; set; }
        public double Level { get; set; }
        public string Currency { get; set; }
        public bool GuaranteedStop { get; set; }
    }

    public class Market
    {
        public string InstrumentName { get; set; }
        public string Expiry { get; set; }
        public string MarketStatus { get; set; }
        public string Epic { get; set; }
        public string InstrumentType { get; set; }
        public int LotSize { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double PercentageChange { get; set; }
        public double NetChange { get; set; }
        public double Bid { get; set; }
        public double Offer { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateTimeUTC { get; set; }
        public int DelayTime { get; set; }
        public bool StreamingPricesAvailable { get; set; }
        public int ScalingFactor { get; set; }
    }

    public class OpenPosition
    {
        public Position Position { get; set; }
        public Market Market { get; set; }
    }

    public class PositionJsonEntity
    {
        public List<OpenPosition> positions { get; set; }
    }

    public class Deal
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string DealStatus {get;set;}
        public string Epic { get; set; }
        public string DealReference { get; set; }
        public string DealId { get; set; }
        public List<AffectedDeal> AffectedDeals { get; set; }
        public double Lavel { get; set; }
        public int Size { get; set; }
        public string Direction { get; set; }
        public bool GuaranteedStop { get; set; }
        public bool TrailingStop { get; set; }
    }

    public class AffectedDeal {
        public string DealId { get; set; }
        public string Status { get; set; }
    }

    public class PositionCreateEntity : PositionUpdateEntity
    {
        public string Direction { get; set; }   //DealDirection enum
        public string Epic { get; set; }    //Instrument epic identificator
        public double Size { get; set; }   //Deal Size
    }

    public class DealRef
    {
        public string DealReference { get; set; }
    }

    public class MarketResponseEntity
    {
        public List<MarketDetail> Markets { get; set; }
    }

    public class MarketDetail
    {
        public int DelayTime { get; set; }
        public string Epic { get; set; }
        public double NetChange { get; set; }
        public int LotSize { get; set; }
        public string Expiry { get; set; }
        public string InstrumentName { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double PercentageChange { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateTimeUTC { get; set; }
        public double Bid { get; set; }
        public double Offer { get; set; }
        public bool StreamingPricesAvailable { get; set; }
        public string MarketStatus { get; set; }
        public int ScalingFactor { get; set; }
    }

    public class PositionUpdateEntity
    {
        public bool? GuaranteedStop { get; set; }
        public bool? TrailingStop { get; set; }
        public int? StopLevel { get; set; }
        public int? StopDistance { get; set; }
        public int? StopAmount { get; set; }
        public int? ProfitLevel { get; set; }
        public int? ProfitDistance { get; set; }
        public int? ProfitAmount { get; set; }
    }

    public class WorkingOrderData
    {
        public string DealId { get; set; }
        public string Direction { get; set; }
        public string Epic { get; set; }
        public int OrderSize { get; set; }
        public int OrderLevel { get; set; }
        public string TimeInForce { get; set; }
        public string GoodTillDate { get; set; }
        public string GoodTillDateUTC { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedDateUTC { get; set; }
        public bool GuaranteedStop { get; set; }
        public string OrderType { get; set; }
        public int StopDistance { get; set; }
        public int ProfitDistance { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class WorkingOrder
    {
        public WorkingOrderData WorkingOrderData { get; set; }
        public Market Market { get; set; }
    }

    public class WorkingOrders
    {
        public List<WorkingOrder> WorkingOrderList { get; set; }
    }

    public class WorkingOrderCreateEntity : PositionCreateEntity
    {
        public int Level { get; set; }  //Order price
        public string Type { get; set; }    //LIMIT or STOP
        public string GoodTillDate { get; set; }    //Order cancellation date in UTC
    }

    public class WorkingOrderUpdateEntity : PositionUpdateEntity
    {
        public int Level { get; set; }  //Order price
        public string GoodTillDate { get; set; }    //Order cancellation date in UTC
    }

    public class Node
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Nodes
    {
        public List<Node> NodeList { get; set; }
    }

    public class Instrument
    {
        public string Epic { get; set; }
        public string Expiry { get; set; }
        public string Name { get; set; }
        public int LotSize { get; set; }
        public string Type { get; set; }
        public bool ControlledRiskAllowed { get; set; }
        public bool StreamingPricesAvailable { get; set; }
        public string Currency { get; set; }
        public int MarginFactor { get; set; }
        public string MarginFactorUnit { get; set; }
        public OpeningHours openingHours { get; set; }
        public string Country { get; set; }
    }

    public class OpeningHours
    {
        public List<string> Mon { get; set; }
        public List<string> Tue { get; set; }
        public List<string> Wed { get; set; }
        public List<string> Thu { get; set; }
        public List<string> Fri { get; set; }
        public List<string> Sat { get; set; }
        public List<string> Sun { get; set; }
        public string Zone { get; set; }
    }

    public class DealingRules
    {
        public UnitValueEntity MinStepDistance { get; set; }
        public UnitValueEntity MinDealSize { get; set; }
        public UnitValueEntity MinControlledRiskStopDistance { get; set; }
        public UnitValueEntity MinNormalStopOrLimitDistance { get; set; }
        public UnitValueEntity MaxStopOrLimitDistance { get; set; }
        public string MarketOrderPreference { get; set; }
        public string TrailingStopsPreference { get; set; }
    }

    public class UnitValueEntity
    {
        public string Unit { get; set; }
        public double Value { get; set; }
    }

    public class Snapshot
    {
        public string MarketStatus { get; set; }
        public double NetChange { get; set; }
        public double PercentageChange { get; set; }
        public string UpdateTime { get; set; }
        public int DelayTime { get; set; }
        public double Bid { get; set; }
        public double Offer { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public int DecimalPlacesFactor { get; set; }
        public int ScalingFactor { get; set; }
    }

    public class MarketDetails
    {
        public Instrument Instrument { get; set; }
        public DealingRules DealingRules { get; set; }
        public Snapshot Snapshot { get; set; }
    }

    public class PriceEntity
    {
        public DateTime SnapshotTime { get; set; }
        public DateTime SnapshotTimeUtc { get; set; }
        public InternalPrice OpenPrice { get; set; }
        public InternalPrice ClosePrice { get; set; }
        public InternalPrice HighPrice { get; set; }
        public InternalPrice LowPrice { get; set; }
        public int LastTradedVolume { get; set; }
    }

    public class InternalPrice
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
    }

    public class PricesEntity
    {
        public List<PriceEntity> Prices { get; set; }
        public string InstrumentType { get; set; }
    }

    public class ClientSentiment
    {
        public string MarketId { get; set; }    //epic
        public double LongPositionPercentage { get; set; }
        public double ShortPositionPercentage { get; set; }
    }

    public class ClientSentiments
    {
        public List<ClientSentiment> ClientSentimentList { get; set; }
    }

    public class Watchlist
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Editable { get; set; }
        public bool Deleteable { get; set; }
        public bool DefaultSystemWatchlist { get; set; }
    }

    public class Watchlists
    {
        public List<Watchlist> WatchlistList { get; set; }
    }

    public class WatchlistCreateEntity
    {
        private string _name;
        public List<string> Epics { get; set; }
        public string Name {
            get
            {
                return _name;
            }
            set
            {
                if (value.Length > 20 || value.Length < 1)
                    throw new Exception("Wrong WatclistCreateEntity Name Lenght");
                _name = value;
            }
        }    //from 1 to 20
    }

    public class WatchlistResultEntity
    {
        public string WatchlistId { get; set; }
        public string Status { get; set; }
    }

    public class EpicNameEntity
    {
        public string Epic { get; set; }
    }
}

