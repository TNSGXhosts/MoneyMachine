using Microsoft.EntityFrameworkCore;

namespace Trading.Application.DAL.Models
{
    [PrimaryKey(nameof(VolumesId))]
    public class TradingVolumes
    {
        public int VolumesId { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }
}