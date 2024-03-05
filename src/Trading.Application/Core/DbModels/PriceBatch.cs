using Core.Models;

namespace Core;

public class PriceBatch
{
    public int PriceBatchId { get;set; }
    public string Ticker { get; set; }
    public string TimeFrame { get; set; }
    public string Period { get; set; }
    public DateTime StartDate { get;set; }
    public DateTime EndDate { get;set; }
    public ICollection<PriceEntity> Prices { get; set; }
}
