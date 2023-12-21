namespace Trading.Application.DAL.Entities
{
    public class PriceEntity
    {
        public int PriceId { get; set; }

        public required string SnapshotTime { get; set; }

        // ReSharper disable once InconsistentNaming
        public required string SnapshotTimeUTC { get; set; }

        public TradingVolumesEntity OpenPrice { get; set; }

        public TradingVolumesEntity ClosePrice { get; set; }

        public TradingVolumesEntity HighPrice { get; set; }

        public TradingVolumesEntity LowPrice { get; set; }

        public double LastTradedVolume { get; set; }

        public required string Ticker { get; set; }

        public required string TimeFrame { get; set; }
    }
}