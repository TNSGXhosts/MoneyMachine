using Microsoft.EntityFrameworkCore;

namespace Trading.Application.DAL.Entities
{
    [PrimaryKey(nameof(VolumesId))]
    public class TradingVolumesEntity
    {
        public int VolumesId { get; set; }

        public double Bid { get; set; }

        public double Ask { get; set; }
    }
}