using Core.Models;
using Trading.Application.Core.APIRequestsEntities;

namespace Core;

public class HistoricalPrices : BaseResponse
{
    public IEnumerable<PriceEntity> Prices { get;set; }
    public string InstrumentType { get; set; }
}
