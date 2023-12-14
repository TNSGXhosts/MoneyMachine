using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Trading.Application;
using Trading.Application.BLL;
using Trading.Application.BLL.CapitalIntegration;
using Trading.Application.DAL;
using Trading.Application.DAL.Data;
using Trading.Application.DAL.DataAccess;
using Trading.Application.Presentation;
using Trading.Application.TelegramIntegration;
using Trading.Application.UserContext;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.RegisterPresentationServices(configuration);
builder.Services.RegisterBusinessLogicLayerServices(configuration);
builder.Services.RegisterDataAccessLayerServices(configuration);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

using var app = builder.Build();

using CancellationTokenSource cts = new();
await RunTelegramBotAsync(app.Services, cts);
RunTradingNotifier(app.Services);

await app.RunAsync();

cts.Cancel();
return;

async Task RunTelegramBotAsync(IServiceProvider hostProvider, CancellationTokenSource cts)
{
    var telegramClient = hostProvider.GetRequiredService<ITelegramClient>();
    await telegramClient.RunAsync(cts);
}

void RunTradingNotifier(IServiceProvider hostProvider)
{
    var tradingNotifier = hostProvider.GetRequiredService<ITradingNotifier>();
    tradingNotifier.Subscribe();
}
