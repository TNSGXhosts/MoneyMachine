using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Trading.Application.Configuration;
using Trading.Application.TelegramConstants;

using Trading.Application.Handlers;

using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.UserContext;

namespace Trading.Application.TelegramIntegration;

// TODO : Refactor this class

internal class TelegramClient(
    ILogger<TelegramClient> logger,
    IOptions<TelegramSettings> options,
    IEnumerable<IHandler> handlers,
    IUserContext userContext) : ITelegramClient
{
    private readonly ILogger<TelegramClient> _logger = logger;
    private readonly TelegramSettings _telegramSettings = options.Value;
    private readonly IEnumerable<IHandler> _handlers = handlers;
    private readonly IUserContext _userContext = userContext;

    private TelegramBotClient? _botClient;
    private int _messageId;

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
                await HandleCallbackQueryAsync(update.CallbackQuery);
            }

            if (update.Message != null)
            {
                await HandleMessageAsync(update.Message);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message} - {e.InnerException}");
        }
    }

    private async Task HandleMessageAsync(Message message)
    {
        if (message.Text != null)
        {
            if (_userContext.IsMessageExpected)
            {
                var hasError = _userContext.ExecuteUserInputPipeline(message.Text);

                var keyboardMarkup = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                });
                var replyMessage = hasError ? "Operation failed" : "Operation have processed successfully";

                _messageId = 0;
                await SendReplyAsync(replyMessage, keyboardMarkup);
            }

            if (message.Text.Equals(nameof(Triggers.Start), StringComparison.OrdinalIgnoreCase))
            {
                _messageId = 0;

                if (_userContext.State != States.Start)
                {
                    _userContext.CatchEvent(Triggers.Start);
                }

                var handler = _handlers.FirstOrDefault(h => h.Trigger == Triggers.Start);
                var reply = handler?.Handle(message.Text);

                if (!string.IsNullOrEmpty(reply?.Item1))
                {
                    await SendReplyAsync(reply.Item1, reply.Item2);
                }
            }
        }
    }

    private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
    {
        Tuple<string, InlineKeyboardMarkup>? reply = null;
        try
        {
            var parsedData = ParseCallbackData(callbackQuery.Data);
            if (parsedData != null)
            {
                _userContext.CatchEvent(parsedData.Item1);

                _logger.LogInformation($"Parsed trigger: {parsedData.Item1}, new state: {_userContext.State}");

                var handler = _handlers.FirstOrDefault(h => h.Trigger == parsedData.Item1);
                reply = handler?.Handle(string.IsNullOrEmpty(parsedData.Item2) ? parsedData.Item1.ToString() : parsedData.Item2);
            }
            else if (_userContext.State != States.Start)
            {
                _logger.LogInformation($"Can't parse callback data as trigger: {callbackQuery.Data} -  used as select");

                var hasError = _userContext.ExecuteUserInputPipeline(callbackQuery.Data);

                reply = new Tuple<string, InlineKeyboardMarkup>(hasError ? "Operation failed" : "Operation have processed successfully",
                    new InlineKeyboardMarkup(new[]
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                        }
                    })
                );
            }

            if (!string.IsNullOrEmpty(reply?.Item1))
            {
                await SendReplyAsync(reply.Item1, reply.Item2);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"{e.Message} - {e.InnerException}");
        }
    }

    private Tuple<Triggers, string>? ParseCallbackData(string data)
    {
        const int parametersMaxLength = 2;
        var parameters = data.Split(ParserConstants.ParserConstants.Separator);

        if (Enum.TryParse(parameters[0], true, out Triggers parsedTrigger) && parameters.Length <= parametersMaxLength)
        {
            return new Tuple<Triggers, string>(parsedTrigger, parameters.Length > 1 ? parameters[1] : string.Empty);
        }

        return null;
    }

    private async Task SendReplyAsync(string message, InlineKeyboardMarkup replyMarkup)
    {
        if (_messageId == 0)
        {
            var firstMessage = await _botClient.SendTextMessageAsync(
                chatId: _telegramSettings.ChatId,
                text: message,
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup);
            _messageId = firstMessage.MessageId;
        }
        else
        {
            await _botClient.EditMessageTextAsync(
                chatId: _telegramSettings.ChatId,
                messageId: _messageId,
                text: message,
                parseMode: ParseMode.Markdown,
                replyMarkup: replyMarkup);
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
