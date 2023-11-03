using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Trading.Application;
using Trading.Application.BLL;
using Trading.Application.DAL;
using Trading.Application.TelegramIntegration;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.RegisterPresentationServices(configuration);
builder.Services.RegisterBusinessLogicLayerServices(configuration);
builder.Services.RegisterDataAccessLayerServices();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

using var app = builder.Build();

await RunTelegramBotAsync(app.Services);

await app.RunAsync();
return;

async Task RunTelegramBotAsync(IServiceProvider hostProvider)
{
    var telegramClient = hostProvider.GetRequiredService<ITelegramClient>();
    await telegramClient.RunAsync();
}
