using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Trading.Application.Configuration;

using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

// TODO : Refactor this class

internal class TelegramClient(
    ILogger<TelegramClient> logger,
    IOptions<TelegramSettings> options,
    IMessageHandler messageHandler,
    ICallbackHandler callbackHandler,
    ITelegramContext telegramContext) : ITelegramClient
{
    private readonly ILogger<TelegramClient> _logger = logger;
    private readonly TelegramSettings _telegramSettings = options.Value;
    private readonly IMessageHandler _messageHandler = messageHandler;
    private readonly ICallbackHandler _callbackHandler = callbackHandler;
    private readonly ITelegramContext _telegramContext = telegramContext;

    private TelegramBotClient? _botClient;

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
        _logger.LogInformation($"Telegram bot client has started with userID: {me.Id} and bot name is {me.FirstName}.");
    }

    public async Task SendMessageAsync(string message, IReplyMarkup replyMarkup = null)
    {
        await _botClient?.SendTextMessageAsync(_telegramSettings.ChatId, message, replyMarkup: replyMarkup)!;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null)
            {
                var replyToUser = _callbackHandler.HandleCallback(update.CallbackQuery);
                await SendReplyAsync(replyToUser.Item1, replyToUser.Item2);
            }

            if (update.Message != null)
            {
                var replyToUser = _messageHandler.HandleMessage(update.Message);
                await SendReplyAsync(replyToUser.Item1, replyToUser.Item2);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message} - {e.InnerException}");
        }
    }

    private async Task SendReplyAsync(string message, InlineKeyboardMarkup replyMarkup)
    {
        if (_botClient != null)
        {
            if (_telegramContext.MessageId == 0)
            {
                var firstMessage = await _botClient.SendTextMessageAsync(
                    chatId: _telegramSettings.ChatId,
                    text: message,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: replyMarkup);
                _telegramContext.MessageId = firstMessage.MessageId;
            }
            else
            {
                await _botClient.EditMessageTextAsync(
                    chatId: _telegramSettings.ChatId,
                    messageId: _telegramContext.MessageId,
                    text: message,
                    parseMode: ParseMode.Markdown,
                    replyMarkup: replyMarkup);
            }
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        _logger.LogError(errorMessage);
        return Task.CompletedTask;
    }
}
