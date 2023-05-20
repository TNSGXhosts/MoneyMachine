using System;
using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using MoneyMachine.BL;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using static MoneyMachine.Tools.Serializator;
using System.Threading;

namespace MoneyMachine.API
{
    public class WebsocketService : BackgroundService
    {
        public static string SecurityTocken;
        public static string CST;
        public static List<string> Epics;
        public static MonitoringManager monitoringManager;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                using (var socket = new ClientWebSocket())
                    try
                    {
                        await socket.ConnectAsync(new Uri(ApplicationConstants.WebSocketUrl), stoppingToken);

                        var subscriber = new MarketDataSubscriber()
                        {
                            Destination = Destinations.SubscribeMarketData,
                            CorrelationId = 1.ToString(),
                            Cst = CST,
                            SecurityToken = SecurityTocken,
                            Payload = new PayLoad() { Epics = Epics }
                        };
                        var jsonString = SerializeObject<MarketDataSubscriber>(subscriber);

                        await Send(socket, jsonString, stoppingToken);
                        await Receive(socket, stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - {ex.Message}");
                    }
        }

        private async Task Send(ClientWebSocket socket, string data, CancellationToken stoppingToken) =>
            await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, stoppingToken);

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            while (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType == WebSocketMessageType.Close)
                        break;

                    ms.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        var response = DeserializeObject<MarketDataUpdateEntity>(await reader.ReadToEndAsync());
                        //Thread updateThread = new Thread(delegate() { monitoringManager.UpdateCurrentValue(response); });
                        //updateThread.Start();
                        monitoringManager.UpdateCurrentValue(response);
                        //Console.WriteLine(response);
                    }
                }
            };
        }
    }
}

