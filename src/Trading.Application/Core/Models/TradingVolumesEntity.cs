using Microsoft.EntityFrameworkCore;

namespace Core.Models;

[PrimaryKey(nameof(VolumesId))]
public class TradingVolumesEntity
{
    public int VolumesId { get; set; }

    public double Bid { get; set; }

    public double Ask { get; set; }
}