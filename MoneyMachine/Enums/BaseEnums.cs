using System;
namespace MoneyMachine.Enums
{
	public enum Status
	{
		OPEN,
		SUCCESS
	}

	public enum DealStatus
	{
		ACCEPTED
	}

	public enum DealDirection
	{
		BUY,
		SELL
	}

	public enum OrderType
	{
		LIMIT,
		STOP
	}

	public enum Resolution
	{
		MINUTE,
		MINUTE_5,
		MINUTE_15,
		MINUTE_30,
		HOUR,
		HOUR_4,
		DAY,
		WEEK
	}
}

