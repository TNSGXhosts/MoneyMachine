using Core;

using Microsoft.Extensions.Options;

using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace Trading.Application.BLL;

public class ReportGenerator(IOptions<ReportGeneratorSettings> reportGeneratorSettings) : IReportGenerator
{
    private readonly string _path = reportGeneratorSettings.Value.ReportPath;

    // private int CountOfTrades
    // {
    //     get => _profitTrades + _loseTrades;
    // }

    private int _profitTrades;
    private int _loseTrades;

    private readonly List<ReportModel> _reportModels = new List<ReportModel>();

    public void ProceedTrade(
        decimal priceChangePercentage,
        DateTime closeDateTime,
        string epic,
        string strategy,
        decimal openPrice,
        decimal closePrice,
        decimal currentBalance)
    {
        if (priceChangePercentage > 0)
        {
            _profitTrades++;
        } else
        {
            _loseTrades++;
        }

        _reportModels.Add(new ReportModel
        {
            Epic = epic,
            Strategy = strategy,
            CloseDateTime = closeDateTime,
            OpenPrice = openPrice,
            ClosePrice = closePrice,
            CurrentBalance = currentBalance
        });
    }

    public void ExportToExcel()
    {
        if (_reportModels.Count != 0)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Testing Report");

                worksheet.Cells[1, 1].Value = nameof(ReportModel.CloseDateTime);
                worksheet.Cells[1, 2].Value = nameof(ReportModel.Epic);
                worksheet.Cells[1, 3].Value = nameof(ReportModel.Strategy);
                worksheet.Cells[1, 4].Value = nameof(ReportModel.OpenPrice);
                worksheet.Cells[1, 5].Value = nameof(ReportModel.ClosePrice);
                worksheet.Cells[1, 6].Value = nameof(ReportModel.CurrentBalance);

                worksheet.Column(1).Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";

                int row = 2;
                foreach (var reportModel in _reportModels)
                {
                    worksheet.Cells[row, 1].Value = reportModel.CloseDateTime;
                    worksheet.Cells[row, 2].Value = reportModel.Epic;
                    worksheet.Cells[row, 3].Value = reportModel.Strategy;
                    worksheet.Cells[row, 4].Value = reportModel.OpenPrice;
                    worksheet.Cells[row, 5].Value = reportModel.ClosePrice;
                    worksheet.Cells[row, 6].Value = reportModel.CurrentBalance;
                    row++;
                }

                var chart = worksheet.Drawings.AddChart("ClosePriceChart", eChartType.LineMarkers);
                chart.SetSize(600, 400);
                chart.SetPosition(0, 0, 7, 0);
                var series = chart.Series.Add(worksheet.Cells["F2:F" + (row - 1)], worksheet.Cells["A2:A" + (row - 1)]);
                series.Header = "Balance";
                chart.Title.Text = "Balance Chart";

                var excelFile = new FileInfo($"{_path}/Report.xlsx");
                excelPackage.SaveAs(excelFile);
            }
        }
    }
}
