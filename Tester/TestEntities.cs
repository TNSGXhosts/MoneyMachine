using System;
using MoneyMachine.Entities;

namespace TestEnv
{
	public class PeriodData
	{
		public PriceEntity UpperPeriod { get; set; }
		public List<PriceEntity> LowerPeriod { get; set; }
    }
}

