// See https://aka.ms/new-console-template for more information
using MoneyMachine.API;
using MoneyMachine.BL;
using MoneyMachine.Entities;
using TestEnv.TestAPI;
using TestEnv.Tester;
using TestEnv.Tools;

Console.WriteLine("Hello, World!");
var testEpic = "NDAQ";

var restApi = new APIProcessor();
var testData = GetTestingData();
var apiEmulator = new APIEmulator(testData);
//var res = restApi.GetMarkets("US Wall Street 30");
var logger = new ExcelLogger();
var monitor = new MonitoringManager(apiEmulator, logger);
for (int i = 20; i < testData.Prices.Count()-1; i++)
    monitor.CheckRegularData();
logger.SaveExcel();
Console.WriteLine($"Start balance: 1000, end balance: {apiEmulator.GetBalance()}");

PricesEntity GetTestingData()
{
    var prices = restApi.GetHistoricalPrices(testEpic, MoneyMachine.Enums.Resolution.DAY, 1000);
    while ((DateTime.UtcNow - DateTime.Parse(prices.Prices.First().SnapshotTime).ToUniversalTime()).Days <= 365)
    {
        var lastDate = DateTime.Parse(prices.Prices.First().SnapshotTime);
        var olderPrices = restApi.GetHistoricalPrices(testEpic, MoneyMachine.Enums.Resolution.DAY, 1000, to: lastDate);
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