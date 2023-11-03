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

    public async Task RunAsync()
    {
        // TODO : Implement Telegram client
        var botClient = new TelegramBotClient(_telegramSettings.AccessToken);
        var me = await botClient.GetMeAsync();
        Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
    }
}
