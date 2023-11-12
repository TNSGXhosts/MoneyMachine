namespace Trading.Application.DAL.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public required string SnapshotTime { get; set; }
        public required string SnapshotTimeUTC { get; set; }
        public TradingVolumes OpenPrice { get; set; }
        public TradingVolumes ClosePrice { get; set; }
        public TradingVolumes HighPrice { get; set; }
        public TradingVolumes LowPrice { get; set; }
        public double LastTradedVolume { get; set; }
    }
}