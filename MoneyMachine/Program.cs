// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MoneyMachine.API;
using MoneyMachine.BL;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using MoneyMachine.Enums;
using MoneyMachine.Tools;
using Newtonsoft.Json;
using RestSharp;

APIProcessor aPIProcessor = null;
try
{
    aPIProcessor = new APIProcessor();
    var session = aPIProcessor.GetSessionInfo();

    var position = new PositionCreateEntity
    {
        Direction = DealDirection.BUY.ToString(),
        Epic = "PGR",
        Size = 1
    };
    //var positions = aPIProcessor.GetAllPositions();
    //var dealRef = aPIProcessor.CreatePosition(position);
    //var markets = aPIProcessor.GetMarkets("Bitcoin");
    //var deal = aPIProcessor.GetDeal(dealRef);
    //aPIProcessor.GetWatchlist("2060331");
    //var prices = aPIProcessor.GetHistoricalPrices("PGR");
    //aPIProcessor.AddMarketToWatchlist("2060331", "SILVER");
    //var market = aPIProcessor.GetSingleMarketDetails("PGR");
    //var clientSentiment = aPIProcessor.GetClientSentiment("PGR");
    //var prices = aPIProcessor.GetHistoricalPrices("SILVER", Resolution.DAY, from: new DateTime(2022, 01, 01), to: new DateTime(2023, 01, 01));
    //var socketClient = new WebSocketClient(aPIProcessor.GetTockens());
    //socketClient.SubscribeMarketData(new List<string>() { "BTCUSD" });
    var accounts = aPIProcessor.GetAccounts();
    var logger = new ConsoleLogger();
    var monitoringManager = new MonitoringManager(aPIProcessor, logger);
    monitoringManager.LoadHistoricalData();

    WebsocketService.SecurityTocken = aPIProcessor.GetTockens().Item1;
    WebsocketService.CST = aPIProcessor.GetTockens().Item2;
    WebsocketService.Epics = new List<string>() { "BTCUSD" };
    WebsocketService.monitoringManager = monitoringManager;

    PingService.SecurityTocken = aPIProcessor.GetTockens().Item1;
    PingService.CST = aPIProcessor.GetTockens().Item2;

    var host = new HostBuilder()
          .ConfigureHostConfiguration(configHost => {
          })
          .ConfigureServices((hostContext, services) => {
              services.AddHostedService<WebsocketService>();
              services.AddHostedService<PingService>();
          })
         .UseConsoleLifetime()
         .Build();

    //run the host
    host.Run();

    aPIProcessor.LogoutSession();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    aPIProcessor.LogoutSession();
}