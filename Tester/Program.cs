// See https://aka.ms/new-console-template for more information
using MoneyMachine.API;
using MoneyMachine.BL;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using TestEnv;
using TestEnv.TestAPI;
using TestEnv.Tester;
using TestEnv.Tools;

Console.WriteLine("Hello, World!");
var testEpic = "TSLA"; //"PGR", "BTCUSD", ICE, JNJ, TXT, PANW, Llus, SPY, US30
var timePeriod = 364 * 5;

var restApi = new APIProcessor();
var testUpperResolutionData = GetTestingData2(Resolution.DAY, DateTime.UtcNow.AddDays(-timePeriod).Date, DateTime.UtcNow.Date);
var testLowerResolutionData = GetTestingPeriodData(Resolution.HOUR, DateTime.UtcNow.AddDays(-timePeriod), DateTime.UtcNow);

//var testUpperResolutionData = GetTestingData(Resolution.DAY, DateTime.UtcNow.AddDays(-timePeriod).Date, DateTime.UtcNow.Date);
//var testLowerResolutionData = GetTestingData(Resolution.HOUR, DateTime.UtcNow.AddDays(-timePeriod), DateTime.UtcNow);
var testData = ConvertData(testUpperResolutionData, testLowerResolutionData);
var apiEmulator = new APIEmulator(testData, Resolution.HOUR, Resolution.DAY);
//var res = restApi.GetMarkets("US Wall Street 30");

var logger = new ExcelLogger();
var monitor = new MonitoringManager(apiEmulator, logger);
for (int i = 20; i < testData.Count(); i++)
{
    for (int j = 0; j < testData[i].LowerPeriod.Count; j++)
    {
        monitor.CheckLowerRegularData(); j += 1;
    }
    monitor.CheckUpperRegularData();
}
logger.SaveExcel();
Console.WriteLine($"Start balance: 1000, end balance: {apiEmulator.GetBalance()}, percent of success: {monitor.PercentSuccess}");

PricesEntity GetTestingData(Resolution resolution, DateTime startDate, DateTime endDate)
{
    var prices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: startDate, to: endDate);
    while ((DateTime.UtcNow.Date - prices.Prices.First().SnapshotTime.ToUniversalTime().Date) < endDate.Date - startDate.Date)
    {
        var lastDate = prices.Prices.First().SnapshotTime;
        var olderPrices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: startDate, to: lastDate);
        var pricesList = new List<PriceEntity>();
        pricesList.AddRange(olderPrices.Prices);
        pricesList.AddRange(prices.Prices);
        prices = new PricesEntity()
        {
            InstrumentType = olderPrices.InstrumentType,
            Prices = pricesList
        };
    }
    return prices;
}

PricesEntity GetTestingData2(Resolution resolution, DateTime startDate, DateTime endDate)
{
    var prices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: startDate);

    var lastDate = prices.Prices.Last().SnapshotTime;
    while (lastDate.Date < endDate.AddDays(-1).Date) {
        lastDate = prices.Prices.Last().SnapshotTime;
        var newPrices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: lastDate);
        prices.Prices.AddRange(newPrices.Prices);
    }
    return prices;
}

PricesEntity GetTestingPeriodData(Resolution resolution, DateTime startDate, DateTime endDate)
{
    var prices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: startDate, to: startDate.AddDays(10) < endDate ? startDate.AddDays(10) : endDate);

    var lastDate = prices.Prices.Last().SnapshotTime;
    while (lastDate < endDate.AddDays(-1))
    {
        lastDate = prices.Prices.Last().SnapshotTime;
        var newPrices = restApi.GetHistoricalPrices(testEpic, resolution, 1000, from: lastDate, to: lastDate.AddDays(10) < endDate ? lastDate.AddDays(10) : endDate);
        prices.Prices.AddRange(newPrices.Prices);
    }
    return prices;
}

List<PeriodData> ConvertData(PricesEntity upperPeriods, PricesEntity lowerPeriods)
{
    var result = new List<PeriodData>();
    foreach(var upperPeriod in upperPeriods.Prices)
    {
        var periodData = new PeriodData()
        {
            UpperPeriod = upperPeriod,
            LowerPeriod = lowerPeriods.Prices.Where(p => p.SnapshotTime.Date == upperPeriod.SnapshotTime.Date).ToList()
        };
        if (periodData.LowerPeriod.Any())
            result.Add(periodData);
    }

    return result;
}

