namespace Trading.Application.DAL.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public required string SnapshotTime { get; set; }
        // ReSharper disable once InconsistentNaming
        public required string SnapshotTimeUTC { get; set; }
        public TradingVolumes OpenPrice { get; set; }
        public TradingVolumes ClosePrice { get; set; }
        public TradingVolumes HighPrice { get; set; }
        public TradingVolumes LowPrice { get; set; }
        public double LastTradedVolume { get; set; }
        public required string Ticker { get; set; }
        public required string TimeFrame { get; set; }
    }
}