using System;
using System.Net.WebSockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MoneyMachine.BL;
using MoneyMachine.Constants;
using MoneyMachine.Entities;
using static MoneyMachine.Tools.Serializator;

namespace MoneyMachine.API
{
    public class PingService : IHostedService, IDisposable
    {
        private Timer _timer;
        public static string SecurityTocken;
        public static string CST;
        public CancellationToken cancellationToken;

        public PingService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service is starting.");

            this.cancellationToken = cancellationToken;
            _timer = new Timer(Ping, null, TimeSpan.Zero,
                TimeSpan.FromMinutes(9));

            return Task.CompletedTask;
        }

        private async void Ping(object state)
        {
            Console.WriteLine("Service is running.");

            if (!cancellationToken.IsCancellationRequested)
                using (var socket = new ClientWebSocket())
                    try
                    {
                        await socket.ConnectAsync(new Uri(ApplicationConstants.WebSocketUrl), cancellationToken);

                        var subscriber = new PingEntity()
                        {
                            Destination = Destinations.PingService,
                            CorrelationId = 1.ToString(),
                            Cst = CST,
                            SecurityToken = SecurityTocken,
                        };
                        var jsonString = SerializeObject<PingEntity>(subscriber);

                        await Send(socket, jsonString, cancellationToken);
                        await Receive(socket, cancellationToken);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR - {ex.Message}");
                    }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private async Task Send(ClientWebSocket socket, string data, CancellationToken stoppingToken) =>
            await socket.SendAsync(Encoding.UTF8.GetBytes(data), WebSocketMessageType.Text, true, stoppingToken);

        private async Task Receive(ClientWebSocket socket, CancellationToken stoppingToken)
        {
            var buffer = new ArraySegment<byte>(new byte[2048]);
            if (!stoppingToken.IsCancellationRequested)
            {
                WebSocketReceiveResult result;
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        result = await socket.ReceiveAsync(buffer, stoppingToken);
                        ms.Write(buffer.Array, buffer.Offset, result.Count);
                    } while (!result.EndOfMessage);

                    if (result.MessageType != WebSocketMessageType.Close)
                    {

                        ms.Seek(0, SeekOrigin.Begin);
                        using (var reader = new StreamReader(ms, Encoding.UTF8))
                        {
                            var response = DeserializeObject<PingResponseEntity>(await reader.ReadToEndAsync());
                            Console.WriteLine($"Ping status: {response}");
                        }
                    }
                }
            };
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}

