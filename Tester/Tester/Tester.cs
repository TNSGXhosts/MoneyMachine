using System;
using MoneyMachine.API;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using NetTrader.Indicator;

namespace TestEnv.Tester
{
	public class Tester
	{
		private APIProcessor APIProcessor;
		private PricesEntity PricesEntity;
		private double Balance = 1000;
        private double PercentForTrade = 0.2;
        private BollingerBandSerie _bollingerBandSerie;
        private RSISerie _rsiSerie;
        private List<Ohlc> OhlcList;
        private const int _rsiUpperBound = 80;
        private const int _rsiLowerBound = 20;
        private PriceEntity openPosition;
        private string CurrentPair = "CHFJPY";//"CHFJPY";//"US30";
        public static PriceEntity PriceEntity;

        public Tester(APIProcessor aPIProcessor)
		{
			APIProcessor = aPIProcessor;
            //PricesEntity = aPIProcessor.GetHistoricalPrices(CurrentPair, MoneyMachine.Enums.Resolution.HOUR, 1000);
            GetYearPrices();
            //OhlcList = PricesEntity.Prices.Select(p => new Ohlc
            //{
            //    High = p.HighPrice.Ask,
            //    Low = p.LowPrice.Ask,
            //    Close = p.ClosePrice.Ask,
            //    Date = DateTime.Parse(p.SnapshotTime)
            //}).ToList();
            //SetBollingerBand(OhlcList);
            //SetRSI(OhlcList);
        }

        public void GetYearPrices()
        {
            var prices = APIProcessor.GetHistoricalPrices(CurrentPair, MoneyMachine.Enums.Resolution.MINUTE_30, 1000);
            while ((DateTime.UtcNow - DateTime.Parse(prices.Prices.First().SnapshotTime).ToUniversalTime()).Days < 365)
            {
                var lastDate = DateTime.Parse(prices.Prices.First().SnapshotTime);
                var olderPrices = APIProcessor.GetHistoricalPrices(CurrentPair, MoneyMachine.Enums.Resolution.MINUTE_30, 1000, to: lastDate);
                var pricesList = new List<PriceEntity>();
                pricesList.AddRange(olderPrices.Prices);
                pricesList.AddRange(prices.Prices);
                prices = new PricesEntity()
                {
                    InstrumentType = olderPrices.InstrumentType,
                    Prices = pricesList
                };
            }
            PricesEntity = prices;
        }

        //public void SetRSI(List<Ohlc> data)
        //{
        //    var rsi = new RSI(14);
        //    rsi.Load(data);
        //    _rsiSerie = rsi.Calculate();
        //}

        //public void SetBollingerBand(List<Ohlc> data)
        //{
        //    var bollingerBand = new BollingerBand(20, 2);
        //    bollingerBand.Load(data);
        //    _bollingerBandSerie = bollingerBand.Calculate();
        //}

        public double RunTest()
        {
            for (int i = 0; i < PricesEntity.Prices.Count(); i++)
            {
                if (_bollingerBandSerie.UpperBand[i].HasValue && _bollingerBandSerie.UpperBand[i] < PricesEntity.Prices[i].ClosePrice.Bid
                    && _rsiSerie.RSI[i].HasValue && _rsiSerie.RSI[i].Value > _rsiUpperBound && openPosition != null)
                {
                    //Sell signal
                    var inPositionSize = Math.Round(Balance * PercentForTrade / openPosition.ClosePrice.Bid, 3);
                    var inPositionValue = inPositionSize * openPosition.ClosePrice.Bid;
                    //Balance = Balance - inPositionValue;
                    var pricesDelta = PricesEntity.Prices[i].ClosePrice.Ask - openPosition.ClosePrice.Bid;
                    var dealResult = pricesDelta * inPositionSize;
                    Balance += dealResult;
                }
                if (_bollingerBandSerie.LowerBand[i].HasValue && _bollingerBandSerie.LowerBand[i].Value > PricesEntity.Prices[i].ClosePrice.Bid
                    && _rsiSerie.RSI[i].HasValue && _rsiSerie.RSI[i].Value < _rsiLowerBound && openPosition == null)
                {
                    //Buy signal
                    openPosition = PricesEntity.Prices[i];
                }
            }

            return Balance;
		}

	}
}

