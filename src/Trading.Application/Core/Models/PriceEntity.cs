namespace Core.Models;

public class PriceEntity
{
    public int PriceId { get; set; }

    public required DateTime SnapshotTime { get; set; }

    // ReSharper disable once InconsistentNaming
    public required DateTime SnapshotTimeUTC { get; set; }

    public TradingVolumesEntity OpenPrice { get; set; }

    public TradingVolumesEntity ClosePrice { get; set; }

    public TradingVolumesEntity HighPrice { get; set; }

    public TradingVolumesEntity LowPrice { get; set; }

    public double LastTradedVolume { get; set; }

    public required string Ticker { get; set; }

    public required string TimeFrame { get; set; }
}