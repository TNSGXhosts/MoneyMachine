using System;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using TestEnv.DBEntities;

namespace TestEnv.Tools
{
	public static class Converter
	{
		public static List<Price> ConvertBrocerPriceToDbPrice(PricesEntity brokerPrice, Resolution resolution, string epic)
		{
			if (brokerPrice == null || !brokerPrice.Prices.Any()) {
				return new List<Price>();
			}

			return brokerPrice.Prices.Select(p => new Price()
			{
				Epic = epic,
				Resolution = resolution.ToString(),
				SnapshotTime = p.SnapshotTime,
				SnapshotTimeUtc = p.SnapshotTimeUtc,
				OpenPriceAsk = p.OpenPrice.Ask,
				OpenPriceBid = p.OpenPrice.Bid,
				ClosePriceAsk = p.ClosePrice.Ask,
				ClosePriceBid = p.ClosePrice.Bid,
				HighPriceAsk = p.HighPrice.Ask,
				HighPriceBid = p.HighPrice.Bid,
				LowPriceAsk = p.LowPrice.Ask,
				LowPriceBid = p.LowPrice.Bid,
				LastTradedVolume = p.LastTradedVolume
			}
			).ToList();
		}
	}
}

