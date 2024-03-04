using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[PrimaryKey(nameof(VolumesId))]
public class TradingVolumesEntity
{
    public int VolumesId { get; set; }

    public decimal Bid { get; set; }

    public decimal Ask { get; set; }
}