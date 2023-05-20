using System;
using MoneyMachine.Constants;
using MoneyMachine.Interface;
using NetTrader.Indicator;

namespace MoneyMachine.Tools
{
	public class ConsoleLogger : ILogger
	{
		public ConsoleLogger()
		{
		}

        public void Log(string field, double? value)
        {
            Console.WriteLine($"{field} - {value}");
        }

        public void LogCurrentData(double BBUpper, double BBLower, double rsi, double bid, double balance)
        {
            Console.WriteLine($"{Fields.BollingerBandUpper}: {BBUpper}");
            Console.WriteLine($"{Fields.BollingerBandLower}: {BBLower}");
            Console.WriteLine($"{Fields.RSI}: {rsi}");
            Console.WriteLine($"{Fields.BID}: {bid}");
            Console.WriteLine($"{Fields.Balance}: {balance}");
        }

        public void LogSignal(bool isBuy, double bid)
        {
            if (isBuy)
                Console.WriteLine($"{Fields.BuySignal}: {bid}");
            else
                Console.WriteLine($"{Fields.SellSignal}: {bid}");
        }
    }
}

