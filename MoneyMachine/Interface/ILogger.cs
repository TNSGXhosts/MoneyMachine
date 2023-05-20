using System;
namespace MoneyMachine.Interface
{
	public interface ILogger
	{
		void Log(string field, double? value);
		void LogCurrentData(double BBUpper, double BBLower, double rsi, double bid, double balance);
		void LogSignal(bool isBuy, double bid);
	}
}

