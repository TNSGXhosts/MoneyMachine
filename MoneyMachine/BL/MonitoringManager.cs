using System;
using MoneyMachine.API;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using MoneyMachine.Interface;
using NetTrader.Indicator;

namespace MoneyMachine.BL
{
	public class MonitoringManager
	{
		private IDataAccess restApiClient;
		private string _tradingPair;
		private int _rsiUpperBound = 70;
		private int _rsiLowerBound = 30;
		private int _bollingerWindow = 20;
		private int _waitTime = 3600;
		private double _percentTrade = 0.5;
		private int _barData = 0;
		private int _latestBarData = 0;
		private double _btcPosition = 0;
		private double _usdPosition = 0;
		private const string _currentPair = "PRG";   //todo:set global configuration for all layers
		public double _balance = 1000;
		private Enums.Resolution LowerResolution = Enums.Resolution.HOUR;
        private Enums.Resolution UpperResolution = Enums.Resolution.DAY;

        private BollingerBandSerie BollingerBandLowerSerie;
		private RSISerie RsiLowerSerie;
        private BollingerBandSerie BollingerBandUpperSerie;
        private RSISerie RsiUpperSerie;
        private OpenPosition openPosition;
		private List<Ohlc> OhlcLowerPeriod;
        private List<Ohlc> OhlcUpperPeriod;
        private ILogger Logger;

		private int lastLog;
		private DateTime lastUpdate;

		private int Minute = 0;
		private double LastDealPrice = 0;
		private int CountSuccessDeals = 0;
		private int CountAllDeals = 0;
		public double PercentSuccess { get { return CountSuccessDeals == 0 ? 0 : (double)CountSuccessDeals/CountAllDeals; } }


        private const int _defaultWindow = 14;

		public MonitoringManager(IDataAccess aPIProcessor, ILogger logger)
		{
			restApiClient = aPIProcessor;
			openPosition = aPIProcessor.GetAllPositions().FirstOrDefault();
			LoadHistoricalData();
			Logger = logger;
		}

		public void SetRSI(bool isLower)
		{
			var rsi = new RSI(_defaultWindow);
			rsi.Load(isLower ? OhlcLowerPeriod : OhlcUpperPeriod);
			if (isLower)
				RsiLowerSerie = rsi.Calculate();
			else
				RsiUpperSerie = rsi.Calculate();
		}

		public void SetBollingerBand(bool isLower)
		{
			var bollingerBand = new BollingerBand(_bollingerWindow, 2);
			bollingerBand.Load(isLower ? OhlcLowerPeriod : OhlcUpperPeriod);
			if (isLower)
				BollingerBandLowerSerie = bollingerBand.Calculate();
			else
				BollingerBandUpperSerie = bollingerBand.Calculate();
		}

		public void LoadHistoricalData()
		{
			//Upper should be first for testing
            var data = restApiClient.GetHistoricalPrices(_currentPair, UpperResolution, _bollingerWindow, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
            OhlcUpperPeriod = data.Prices.Select(p => new Ohlc
            {
                High = p.HighPrice.Ask,
                Low = p.LowPrice.Ask,
                Close = p.ClosePrice.Ask,
                Date = p.SnapshotTime
            }).ToList();
            SetBollingerBand(false);
            SetRSI(false);
            lastUpdate = DateTime.UtcNow;
			//for test
            data = restApiClient.GetHistoricalPricesHourPeriodForTesting(_currentPair, LowerResolution, _bollingerWindow, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
            OhlcLowerPeriod = data.Prices.Select(p => new Ohlc
            {
                High = p.HighPrice.Ask,
                Low = p.LowPrice.Ask,
                Close = p.ClosePrice.Ask,
                Date = p.SnapshotTime
            }).ToList();
            SetBollingerBand(true);
            SetRSI(true);
        }

		public void UpdateCurrentValue(MarketDataUpdateEntity update)
		{
			var currentMinute = DateTime.UtcNow.Minute;
			if (currentMinute % 5 == 0 && currentMinute != lastLog)
			{
				var data = restApiClient.GetHistoricalPrices(_currentPair, Enums.Resolution.MINUTE_5, 1, DateTime.UtcNow.AddMinutes(-10), DateTime.UtcNow);
				var isDataValid = data.Prices.LastOrDefault() != null;

				if (isDataValid)
				{
					OhlcLowerPeriod.Add(new Ohlc()
					{
						High = data.Prices.LastOrDefault().HighPrice.Ask,
						Low = data.Prices.LastOrDefault().LowPrice.Ask,
						Close = data.Prices.LastOrDefault().ClosePrice.Ask,
						Date = data.Prices.LastOrDefault().SnapshotTime
					});
					SetBollingerBand(true);
					SetRSI(true);
				}

				lastLog = currentMinute;
				
				Console.WriteLine($"Bollinger Band Upper: {BollingerBandUpperSerie.UpperBand.LastOrDefault()}");
				Console.WriteLine($"Bollinger Band Lower: {BollingerBandUpperSerie.LowerBand.LastOrDefault()}");
				Console.WriteLine($"RSI: {RsiUpperSerie.RSI.LastOrDefault().Value}");
				Console.WriteLine($"Current bid: {update.Payload.Ofr}");

				if (BollingerBandUpperSerie.UpperBand.LastOrDefault().HasValue && BollingerBandUpperSerie.UpperBand.LastOrDefault().Value < update.Payload.Bid
					&& RsiUpperSerie.RSI.LastOrDefault().HasValue && RsiUpperSerie.RSI.LastOrDefault().Value > _rsiUpperBound && openPosition != null)
				{
					//Sell signal
					Console.WriteLine($"Sell signal, bid: {update.Payload.Bid}");
					restApiClient.ClosePosition(openPosition.Position.DealId);
				}
				if (BollingerBandUpperSerie.LowerBand.LastOrDefault().HasValue && BollingerBandUpperSerie.LowerBand.LastOrDefault().Value > update.Payload.Bid
					&& RsiUpperSerie.RSI.LastOrDefault().HasValue && RsiUpperSerie.RSI.LastOrDefault().Value < _rsiLowerBound && openPosition == null)
				{
					//Buy signal
                    Console.WriteLine($"Buy signal, bid: {update.Payload.Bid}");
					var position = new PositionCreateEntity()
					{
						Direction = DealDirection.BUY.ToString(),
						Epic = _currentPair,
						Size = Math.Round(_percentTrade * _balance / update.Payload.Bid, 3)
					};
					restApiClient.CreatePosition(position);
					openPosition = restApiClient.GetAllPositions().FirstOrDefault();
				}
				if (isDataValid)
				{
					OhlcLowerPeriod.Remove(OhlcLowerPeriod.LastOrDefault());
				}
			}
			if (DateTime.UtcNow.Hour - lastUpdate.Hour >= 1)
				LoadHistoricalData();
        }

		public void CheckUpperRegularData()
		{
			CheckRegularData(UpperResolution, false);
		}

        public void CheckLowerRegularData()
        {
			CheckRegularData(LowerResolution, true);
        }

		public void CheckRegularData(Resolution resolution, bool isLower)
		{
            var data = restApiClient.GetHistoricalPrices(_currentPair, resolution, 1, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
            var isDataValid = data.Prices?.LastOrDefault() != null;

            if (isDataValid)
            {
                var update = new Ohlc()
                {
                    High = data.Prices.LastOrDefault().HighPrice.Bid,
                    Low = data.Prices.LastOrDefault().LowPrice.Bid,
                    Close = data.Prices.LastOrDefault().ClosePrice.Bid,
                    Date = data.Prices.LastOrDefault().SnapshotTime
                };

				//ohlcList.Remove(ohlcList.First());//todo:???
				if (isLower)
					OhlcLowerPeriod.Add(update);
				else
					OhlcUpperPeriod.Add(update);
                SetBollingerBand(isLower);
                SetRSI(isLower);

				if (isLower)
				{
					if (CheckSellSignal(update.Close))
					{
						//Sell signal
						Logger.LogSignal(false, update.Close);
						restApiClient.ClosePosition(openPosition.Position.DealId);
						openPosition = null;
						if (update.Close > LastDealPrice)
							CountSuccessDeals = ++CountSuccessDeals;
						LastDealPrice = 0;
						CountAllDeals = ++CountAllDeals;
						//Logger.Log(Fields.PercentSuccessTrades, CountSuccessDeals == 0 ? 0 : CountAllDeals /CountSuccessDeals);
					}
					if (CheckBuySignal(update.Close))
					{
						//Buy signal
						Logger.LogSignal(true, update.Close);
						var position = new PositionCreateEntity()
						{
							Direction = DealDirection.BUY.ToString(),
							Epic = _currentPair,
							Size = Math.Round(_percentTrade * _balance / update.Close, 0) //(int)Math.Round(_percentTrade * _balance / update.Close)
						};
						restApiClient.CreatePosition(position);
						openPosition = restApiClient.GetAllPositions().FirstOrDefault();
						LastDealPrice = update.Close;
					}
					_balance = restApiClient.GetBalance();

					Logger.LogCurrentData(BollingerBandLowerSerie.UpperBand.LastOrDefault().Value, BollingerBandLowerSerie.LowerBand.LastOrDefault().Value, RsiLowerSerie.RSI.LastOrDefault().Value, update.Close, _balance, update.Date);
				}
			}
        }

		private bool CheckSellSignal(double currentPrice)
		{
			return ((BollingerBandUpperSerie.UpperBand.LastOrDefault().HasValue && BollingerBandUpperSerie.UpperBand.LastOrDefault().Value < currentPrice)
				/*|| (BollingerBandLowerSerie.UpperBand.LastOrDefault().HasValue && BollingerBandLowerSerie.UpperBand.LastOrDefault().Value < currentPrice)*/)
                && CheckSellByRSI();
        }

        private bool CheckBuySignal(double currentPrice)
		{
			return ((BollingerBandUpperSerie.LowerBand.LastOrDefault().HasValue && BollingerBandUpperSerie.LowerBand.LastOrDefault().Value > currentPrice)
				&& (BollingerBandLowerSerie.LowerBand.LastOrDefault().HasValue && BollingerBandLowerSerie.LowerBand.LastOrDefault().Value > currentPrice))
                && CheckBuyByRSI();

        }

        private bool CheckSellByRSI()
		{
			return ((RsiUpperSerie.RSI.LastOrDefault().HasValue && RsiUpperSerie.RSI.LastOrDefault().Value > _rsiUpperBound)
				/*|| (RsiLowerSerie.RSI.LastOrDefault().HasValue && RsiLowerSerie.RSI.LastOrDefault().Value > _rsiUpperBound)*/) && openPosition != null;
        }

        private bool CheckBuyByRSI()
		{
			return ((RsiUpperSerie.RSI.LastOrDefault().HasValue && RsiUpperSerie.RSI.LastOrDefault().Value < _rsiLowerBound)
				&& (RsiLowerSerie.RSI.LastOrDefault().HasValue && RsiLowerSerie.RSI.LastOrDefault().Value < _rsiLowerBound)) && openPosition == null;
        }
    }
}

