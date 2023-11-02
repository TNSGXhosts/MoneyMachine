using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Trading.Application;
using Trading.Application.BLL;
using Trading.Application.TelegramIntegration;

var builder = Host.CreateApplicationBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

builder.Services.ConfigurationRegistry(configuration);
builder.Services.RegisterPresentationServices();
builder.Services.RegisterBusinessLogicLayerServices();

using var host = builder.Build();

await RunTelegramBotAsync(host.Services);

await host.RunAsync();

async Task RunTelegramBotAsync(IServiceProvider hostProvider)
{
    var telegramClient = hostProvider.GetRequiredService<ITelegramClient>();
    await telegramClient.RunAsync();
}
