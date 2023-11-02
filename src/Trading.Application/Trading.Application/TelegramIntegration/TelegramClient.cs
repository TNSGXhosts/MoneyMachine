using Microsoft.Extensions.Options;

using Telegram.Bot;

using Trading.Application.Configuration;

namespace Trading.Application.TelegramIntegration;

internal class TelegramClient : ITelegramClient
{
    private readonly TelegramSettings _telegramSettings;

    public TelegramClient(IOptions<TelegramSettings> options)
    {
        _telegramSettings = options.Value;
    }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
    public async Task RunAsync()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
    {
        // TODO : Implement Telegram client
        var botClient = new TelegramBotClient(_telegramSettings.AccessToken);
        var me = botClient.GetMeAsync().Result;
        Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
    }
}
