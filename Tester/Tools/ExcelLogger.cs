using System;
using MoneyMachine.Interface;
using Aspose.Cells;
using MoneyMachine.Constants;
using System.Configuration;
using System.Security.Cryptography;
using NetTrader.Indicator;

namespace TestEnv.Tools
{
	public class ExcelLogger : ILogger
	{
        private readonly string DataPath = ConfigurationManager.AppSettings.Get("DataPath");
        private Workbook Workbook = new Workbook();
        private Worksheet MainSheet;
        private int Index = 1;


        public ExcelLogger()
		{
            // When you create a new workbook, a default "Sheet1" is added to the workbook.
            MainSheet = Workbook.Worksheets[0];

            // Input the "Hello World!" text into the "A1" cell
            MainSheet.Cells[$"A{Index}"].PutValue(Fields.BID);
            MainSheet.Cells[$"B{Index}"].PutValue(Fields.Balance);
            MainSheet.Cells[$"C{Index}"].PutValue(Fields.BollingerBandLower);
            MainSheet.Cells[$"D{Index}"].PutValue(Fields.BollingerBandUpper);
            MainSheet.Cells[$"E{Index}"].PutValue(Fields.RSI);
            MainSheet.Cells[$"F{Index}"].PutValue(Fields.SellSignal);
            MainSheet.Cells[$"G{Index}"].PutValue(Fields.BuySignal);
            MainSheet.Cells[$"I{Index}"].PutValue(Fields.Date);
            MainSheet.Cells[$"J{Index}"].Value = Fields.ProductPriceChanging;
            MainSheet.Cells[$"K{Index}"].Value = Fields.BalanceChanging;
            Index = ++Index;
        }

        public void Log(string field, double? value)
        {
            MainSheet.Cells[$"H{Index}"].PutValue(value);
        }

        public void SaveExcel()
        {
            int chartIndex = MainSheet.Charts.Add(Aspose.Cells.Charts.ChartType.Line, 1, 11, 50, 31);
            // Accessing the instance of the newly added chart
            Aspose.Cells.Charts.Chart chart = MainSheet.Charts[chartIndex];
            // Setting chart data source as the range  "A1:C4"
            if(Index > 2)
                chart.SetChartDataRange($"J2:K{Index}", true);

            // Save the Excel file.
            Workbook.Save(Path.Join(DataPath, "DataSet.xlsx"));
        }

        public void LogCurrentData(double BBUpper, double BBLower, double rsi, double bid, double balance, DateTime date)
        {
            MainSheet.Cells[$"A{Index}"].Value = bid;
            MainSheet.Cells[$"B{Index}"].Value = balance;
            MainSheet.Cells[$"C{Index}"].PutValue(BBLower);
            MainSheet.Cells[$"D{Index}"].PutValue(BBUpper);
            MainSheet.Cells[$"E{Index}"].PutValue(rsi);
            MainSheet.Cells[$"I{Index}"].PutValue(date.ToString());
            MainSheet.Cells[$"J{Index}"].Value = bid / (Convert.ToDouble(MainSheet.Cells[$"A{17}"].Value) / 100);
            MainSheet.Cells[$"K{Index}"].Value = balance / (Convert.ToDouble(MainSheet.Cells[$"B{2}"].Value) / 100);
            Index = ++Index;
        }

        public void LogSignal(bool isBuy, double bid)
        {
            if (isBuy)
                MainSheet.Cells[$"G{Index}"].PutValue(bid);
            else
                MainSheet.Cells[$"F{Index}"].PutValue(bid);
        }
    }
}

