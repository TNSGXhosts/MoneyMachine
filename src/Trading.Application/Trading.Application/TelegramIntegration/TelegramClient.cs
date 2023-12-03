using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using Stateless;

using Trading.Application.Configuration;
using Trading.Application.TelegramConstants;

using Trading.Application.Handlers;

using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

internal class TelegramClient(
    ILogger<TelegramClient> logger,
    IOptions<TelegramSettings> options,
    IEnumerable<IHandler> handlers) : ITelegramClient
{
    private TelegramBotClient? _botClient;
    private readonly StateMachine<States, Triggers> _machine = new StateMachine<States, Triggers>(States.Start);

    private int _messageId;
    private readonly TelegramSettings _telegramSettings = options.Value;

    public async Task RunAsync(CancellationTokenSource cts)
    {
        _botClient = new TelegramBotClient(_telegramSettings.AccessToken);

        ConfigureStateMachine();

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

    private void ConfigureStateMachine() {
        _machine.Configure(States.Start).Permit(Triggers.AddOrder, States.CreationOrder);
        _machine.Configure(States.Start).Permit(Triggers.ChooseToUpdate, States.UpdatingOrder);
        _machine.Configure(States.Start).Permit(Triggers.ChooseToClose, States.ChooseOrderToClose);
        _machine.Configure(States.CreationOrder).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.UpdatingOrder).Permit(Triggers.Start, States.Start);
        _machine.Configure(States.ChooseOrderToClose).Permit(Triggers.Start, States.Start);
    }

    public async Task SendMessageAsync(string message)
    {
        await _botClient?.SendTextMessageAsync(_telegramSettings.ChatId, message)!;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try {
            if (update.Type == UpdateType.CallbackQuery && update.CallbackQuery != null) {
                HandleCallbackQuery(update.CallbackQuery);
            }

            if (update.Message != null) {
                HandleMessage(update.Message);
            }
        } catch(Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
        }
    }

    private async void HandleMessage(Message message)
    {
        try {
            if (message.Text != null)
            {
                if (Enum.TryParse(message.Text, true, out Triggers parsedTrigger) && _machine.State != States.Start)
                {
                    _machine.Fire(parsedTrigger);
                    _messageId = 0;
                }

                var handler = handlers.FirstOrDefault(h => h.State == _machine.State);
                var reply = handler?.HandleMessage(message.Text);
                if (!string.IsNullOrEmpty(reply?.Item1)){
                    if (_messageId == 0){
                        var firstMessage = await _botClient.SendTextMessageAsync(
                            chatId: _telegramSettings.ChatId,
                            text: reply.Item1,
                            replyMarkup: reply.Item2
                        );
                        _messageId = firstMessage.MessageId;
                    } else {
                        await _botClient.EditMessageTextAsync(
                            chatId: _telegramSettings.ChatId,
                            messageId:_messageId,
                            text: reply.Item1,
                            replyMarkup: reply.Item2);
                    }
                } else {
                    logger.LogError($"Can't find a handler for: {_machine.State}");
                }
            }
        } catch (Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
        }
    }

    private async void HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        Tuple<string, InlineKeyboardMarkup> reply;
        try {
            if (Enum.TryParse(callbackQuery.Data, true, out Triggers parsedTrigger))
            {
                _machine.Fire(parsedTrigger);

                logger.LogInformation($"Parsed trigger: {parsedTrigger}, new state: {_machine.State}");

                var handler = handlers.FirstOrDefault(h => h.State == _machine.State);
                reply = handler?.HandleCallBack(string.Empty);
            }
            else
            {
                logger.LogError($"Can't parse callback data as trigger: {callbackQuery.Data} -  used as select");

                var handler = handlers.FirstOrDefault(h => h.State == _machine.State);
                reply = handler?.HandleCallBackSelect(callbackQuery.Data);
            }

            if (!string.IsNullOrEmpty(reply?.Item1)) {
                if (_messageId == 0) {
                    var firstMessage = await _botClient.SendTextMessageAsync(
                        chatId: _telegramSettings.ChatId,
                        text: reply.Item1,
                        replyMarkup: reply.Item2
                    );
                    _messageId = firstMessage.MessageId;
                } else {
                    await _botClient.EditMessageTextAsync(
                                chatId: _telegramSettings.ChatId,
                                messageId:_messageId,
                                text: reply.Item1,
                                replyMarkup: reply.Item2);
                }
            }
        } catch(Exception e) {
            logger.LogError($"{e.Message} - {e.InnerException}");
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

        logger.LogError(errorMessage);
        return Task.CompletedTask;
    }
}
