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
		private Enums.Resolution DefaultResolution = Enums.Resolution.HOUR;

		private BollingerBandSerie _bollingerBandSerie;
		private RSISerie _rsiSerie;
		private OpenPosition openPosition;
		private List<Ohlc> ohlcList;
		private ILogger Logger;

		private int lastLog;
		private DateTime lastUpdate;

		private int Minute = 0;


        private const int _defaultWindow = 14;

		public MonitoringManager(IDataAccess aPIProcessor, ILogger logger)
		{
			restApiClient = aPIProcessor;
			openPosition = aPIProcessor.GetAllPositions().FirstOrDefault();
			LoadHistoricalData();
			Logger = logger;
		}

		public void SetRSI(List<Ohlc> data)
		{
			var rsi = new RSI(_defaultWindow);
			rsi.Load(data);
			_rsiSerie = rsi.Calculate();
		}

		public void SetBollingerBand(List<Ohlc> data)
		{
			var bollingerBand = new BollingerBand(_bollingerWindow, 2);
			bollingerBand.Load(data);
			_bollingerBandSerie = bollingerBand.Calculate();
		}

		public void LoadHistoricalData()
		{
			var data = restApiClient.GetHistoricalPrices(_currentPair, DefaultResolution, _bollingerWindow, DateTime.UtcNow.AddMonths(-1), DateTime.UtcNow);
			ohlcList = data.Prices.Select(p => new Ohlc
			{
				High = p.HighPrice.Ask,
				Low = p.LowPrice.Ask,
				Close = p.ClosePrice.Ask,
				Date = DateTime.Parse(p.SnapshotTime)
			}).ToList();
            SetBollingerBand(ohlcList);
            SetRSI(ohlcList);
			lastUpdate = DateTime.UtcNow;
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
					ohlcList.Add(new Ohlc()
					{
						High = data.Prices.LastOrDefault().HighPrice.Ask,
						Low = data.Prices.LastOrDefault().LowPrice.Ask,
						Close = data.Prices.LastOrDefault().ClosePrice.Ask,
						Date = DateTime.Parse(data.Prices.LastOrDefault().SnapshotTime)
					});
					SetBollingerBand(ohlcList);
					SetRSI(ohlcList);
				}

				lastLog = currentMinute;
				
				Console.WriteLine($"Bollinger Band Upper: {_bollingerBandSerie.UpperBand.LastOrDefault()}");
				Console.WriteLine($"Bollinger Band Lower: {_bollingerBandSerie.LowerBand.LastOrDefault()}");
				Console.WriteLine($"RSI: {_rsiSerie.RSI.LastOrDefault().Value}");
				Console.WriteLine($"Current bid: {update.Payload.Ofr}");

				if (_bollingerBandSerie.UpperBand.LastOrDefault().HasValue && _bollingerBandSerie.UpperBand.LastOrDefault().Value < update.Payload.Bid
					&& _rsiSerie.RSI.LastOrDefault().HasValue && _rsiSerie.RSI.LastOrDefault().Value > _rsiUpperBound && openPosition != null)
				{
					//Sell signal
					Console.WriteLine($"Sell signal, bid: {update.Payload.Bid}");
					restApiClient.ClosePosition(openPosition.Position.DealId);
				}
				if (_bollingerBandSerie.LowerBand.LastOrDefault().HasValue && _bollingerBandSerie.LowerBand.LastOrDefault().Value > update.Payload.Bid
					&& _rsiSerie.RSI.LastOrDefault().HasValue && _rsiSerie.RSI.LastOrDefault().Value < _rsiLowerBound && openPosition == null)
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
					ohlcList.Remove(ohlcList.LastOrDefault());
				}
			}
			if (DateTime.UtcNow.Hour - lastUpdate.Hour >= 1)
				LoadHistoricalData();
        }

        public void CheckRegularData()
        {
            var data = restApiClient.GetHistoricalPrices(_currentPair, DefaultResolution, 1, DateTime.UtcNow.AddHours(-1), DateTime.UtcNow);
            var isDataValid = data.Prices?.LastOrDefault() != null;

			if (isDataValid)
			{
				var update = new Ohlc()
				{
					High = data.Prices.LastOrDefault().HighPrice.Ask,
					Low = data.Prices.LastOrDefault().LowPrice.Ask,
					Close = data.Prices.LastOrDefault().ClosePrice.Ask,
					Date = DateTime.Parse(data.Prices.LastOrDefault().SnapshotTime)
				};

				//ohlcList.Remove(ohlcList.First());//todo:???
				ohlcList.Add(update);
				SetBollingerBand(ohlcList);
				SetRSI(ohlcList);

				if (CheckSellSignal(update.Close))
				{
					//Sell signal
					Logger.LogSignal(false, update.Close);
					restApiClient.ClosePosition(openPosition.Position.DealId);
					_balance = restApiClient.GetBalance();
					openPosition = null;
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
					_balance = restApiClient.GetBalance();
				}

                Logger.LogCurrentData(_bollingerBandSerie.UpperBand.LastOrDefault().Value, _bollingerBandSerie.LowerBand.LastOrDefault().Value, _rsiSerie.RSI.LastOrDefault().Value, update.Close, _balance);
            }
        }

		private bool CheckSellSignal(double currentPrice)
		{
			return _bollingerBandSerie.UpperBand.LastOrDefault().HasValue && _bollingerBandSerie.UpperBand.LastOrDefault().Value < currentPrice
				&& CheckSellByRSI();
        }

        private bool CheckBuySignal(double currentPrice)
		{
			return _bollingerBandSerie.LowerBand.LastOrDefault().HasValue && _bollingerBandSerie.LowerBand.LastOrDefault().Value > currentPrice
				&& CheckBuyByRSI();

        }

        private bool CheckSellByRSI()
		{
			return _rsiSerie.RSI.LastOrDefault().HasValue && _rsiSerie.RSI.LastOrDefault().Value > _rsiUpperBound && openPosition != null;
        }

        private bool CheckBuyByRSI()
		{
			return _rsiSerie.RSI.LastOrDefault().HasValue && _rsiSerie.RSI.LastOrDefault().Value < _rsiLowerBound && openPosition == null;
        }
    }
}

