using System;
using System.Configuration;
using MoneyMachine.API;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using MoneyMachine.Interface;

namespace TestEnv.TestAPI
{
	public class APIEmulator : IDataAccess
	{
        private List<OpenPosition> Positions;
        private List<PeriodData> PeriodDatas;
        private int LowerResolutionIndex;
        private int UpperResolutionIndex;
        private int DealCount;
        private double Balance;
        private Resolution LowerResolution;
        private Resolution UpperResolution;
        private string ConnectionString;

		public APIEmulator(List<PeriodData> periodDatas, Resolution lowerResolution, Resolution upperResolution)
		{
            PeriodDatas = periodDatas;
            LowerResolution = lowerResolution;
            UpperResolution = upperResolution;
            Positions = new List<OpenPosition>();
            LowerResolutionIndex = 0;
            UpperResolutionIndex = 0;
            Balance = 1000;
            ConnectionString = ConfigurationManager.AppSettings.Get(ConfigurationKeys.ConnectionString);
        }

        public string ClosePosition(string dealId)
        {
            var closingPosition = Positions.FirstOrDefault(p => p.Position.DealId == dealId);
            if (closingPosition != null)
            {
                Positions.Remove(closingPosition);
                Balance += closingPosition.Position.Size * PeriodDatas[UpperResolutionIndex].LowerPeriod[LowerResolutionIndex].ClosePrice.Bid;
            }
            else
            {
                Console.WriteLine("Closing position not found");
            }

            return string.Empty;
        }

        public string CreatePosition(PositionCreateEntity positionSetup)
        {
            DealCount = DealCount++;
            Positions.Add(new OpenPosition()
            {
                Market = new Market()
                {
                    Epic = positionSetup.Epic,
                    Bid = PeriodDatas[UpperResolutionIndex].LowerPeriod[LowerResolutionIndex].ClosePrice.Bid
                },
                Position = new Position()
                {
                    DealId = DealCount.ToString(),
                    Size = positionSetup.Size,
                    Direction = positionSetup.Direction
                }
            });
            Balance -= positionSetup.Size * PeriodDatas[UpperResolutionIndex].LowerPeriod[LowerResolutionIndex].ClosePrice.Ask;

            return DealCount.ToString();
        }

        public List<OpenPosition> GetAllPositions()
        {
            return Positions;
        }

        public PricesEntity GetHistoricalPrices(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null)
        {
            if (PeriodDatas == null || PeriodDatas.Count == 0)
                throw new Exception("No prices");

            var result = new PricesEntity()
            {
                InstrumentType = epic
            };
            if (resolution == LowerResolution)
            {
                //var count = PeriodDatas[UpperResolutionIndex].LowerPeriod.Count - 1 >= LowerResolutionIndex + max.Value ? max.Value : PeriodDatas[UpperResolutionIndex].LowerPeriod.Count - 1 - LowerResolutionIndex;
                if (LowerResolutionIndex < PeriodDatas[UpperResolutionIndex].LowerPeriod.Count /*&& PeriodDatas[UpperResolutionIndex].LowerPeriod.Count - 1 >= LowerResolutionIndex + max.Value*/)
                {
                    result.Prices = new List<PriceEntity>(PeriodDatas[UpperResolutionIndex].LowerPeriod.GetRange(LowerResolutionIndex, max.Value));
                    LowerResolutionIndex = LowerResolutionIndex + max.Value + 1 >= PeriodDatas[UpperResolutionIndex].LowerPeriod.Count ? 0 : LowerResolutionIndex + max.Value;//todo:check
                }
            }
            else
            {
                if (UpperResolutionIndex < PeriodDatas.Count && PeriodDatas.Count - 1 >= UpperResolutionIndex + max.Value)
                {
                    result.Prices = new List<PriceEntity>(PeriodDatas.GetRange(UpperResolutionIndex + 1, max.Value).Select(p => p.UpperPeriod));
                    UpperResolutionIndex += max.Value;//check
                }
            }

            return result;
        }

        public PricesEntity GetHistoricalPricesHourPeriodForTesting(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null)
        {
            if (PeriodDatas == null || PeriodDatas.Count == 0)
                throw new Exception("No prices");

            var result = new PricesEntity()
            {
                InstrumentType = epic
            };
            if (resolution == LowerResolution)//for lower only
            {
                var range = PeriodDatas.GetRange(UpperResolutionIndex - 5, 4);
                var i = 1;
                var lowerRange = range.First().LowerPeriod;
                while(lowerRange.Count < 20)
                {
                    lowerRange.AddRange(range[i].LowerPeriod);
                    i += 1;
                }
                result.Prices = lowerRange;
            }

            return result;
        }

        public double GetBalance()
        {
            return Balance + Positions.Select(p => PeriodDatas[UpperResolutionIndex].LowerPeriod[LowerResolutionIndex].ClosePrice.Bid * p.Position.Size).Sum();
        }
    }
}

