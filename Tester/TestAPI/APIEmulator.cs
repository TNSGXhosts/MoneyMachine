using System;
using MoneyMachine.API;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using MoneyMachine.Interface;

namespace TestEnv.TestAPI
{
	public class APIEmulator : IDataAccess
	{
        private PricesEntity PricesEntity;
        private List<OpenPosition> Positions;
        private int Index;
        private int DealCount;
        private double Balance;

		public APIEmulator(PricesEntity pricesEntity)
		{
            PricesEntity = pricesEntity;
            Positions = new List<OpenPosition>();
            Index = 0;
            Balance = 1000;
		}

        public string ClosePosition(string dealId)
        {
            var closingPosition = Positions.FirstOrDefault(p => p.Position.DealId == dealId);
            if (closingPosition != null)
            {
                Positions.Remove(closingPosition);
                Balance += closingPosition.Position.Size * PricesEntity.Prices[Index - 1].ClosePrice.Bid;
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
                    Bid = PricesEntity.Prices[Index - 1].ClosePrice.Bid
                },
                Position = new Position()
                {
                    DealId = DealCount.ToString(),
                    Size = positionSetup.Size,
                    Direction = positionSetup.Direction
                }
            });
            Balance -= positionSetup.Size * PricesEntity.Prices[Index - 1].ClosePrice.Ask;

            return DealCount.ToString();
        }

        public List<OpenPosition> GetAllPositions()
        {
            return Positions;
        }

        public PricesEntity GetHistoricalPrices(string epic, Resolution? resolution = null, int? max = null, DateTime? from = null, DateTime? to = null)
        {
            if (PricesEntity?.Prices == null || PricesEntity.Prices.Count == 0)
                throw new Exception("No prices");

            var result = new PricesEntity()
            {
                InstrumentType = epic
            };
            if (Index <= PricesEntity.Prices.Count && PricesEntity.Prices.Count >= Index + max.Value)
            {
                result.Prices = new List<PriceEntity>(PricesEntity.Prices.GetRange(Index, max.Value));
                Index += max.Value;//check
            }
            return result;
        }

        public double GetBalance()
        {
            return Balance + Positions.Select(p => PricesEntity.Prices[Index - 1].ClosePrice.Bid * p.Position.Size).Sum();
        }
    }
}

