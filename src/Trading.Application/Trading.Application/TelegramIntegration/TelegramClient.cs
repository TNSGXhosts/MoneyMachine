using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Trading.Application.Configuration;

namespace Trading.Application.TelegramIntegration;

internal class TelegramClient(ILogger<TelegramClient> logger, IOptions<TelegramSettings> options) : ITelegramClient
{
    private TelegramBotClient? _botClient;

    private readonly TelegramSettings _telegramSettings = options.Value;

    public async Task RunAsync(CancellationTokenSource cts)
    {
        _botClient = new TelegramBotClient(_telegramSettings.AccessToken);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
            pollingErrorHandler: HandlePollingErrorAsync,
            receiverOptions: receiverOptions,
            cancellationToken: cts.Token
        );

        var me = await _botClient.GetMeAsync();
        logger.LogInformation($"Telegram bot client has started with userID: {me.Id} and bot name is {me.FirstName}.");
    }

    public async Task SendMessageAsync(string message)
    {
        await _botClient?.SendTextMessageAsync(_telegramSettings.ChatId, message)!;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
        {
            return;
        }

        if (message.Text is not { } messageText)
        {
            return;
        }

        var chatId = message.Chat.Id;

        logger.LogInformation($"Received a '{messageText}' message in chat {chatId}.");

        await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Your chatId:\n" + chatId,
            cancellationToken: cancellationToken);
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        logger.LogError(errorMessage);
        return Task.CompletedTask;
    }
}
