using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.Handlers;
using Trading.Application.TelegramConstants;
using Trading.Application.UserContext;

namespace Trading.Application.TelegramIntegration;

public class MessageHandler(IUserContext userContext,
                    ITelegramContext telegramContext,
                    IEnumerable<IHandler> handlers) : IMessageHandler
{
    public Tuple<string, InlineKeyboardMarkup> HandleMessage(Message message)
    {
        if (message.Text != null)
        {
            if (userContext.IsMessageExpected)
            {
                var hasError = userContext.ExecuteUserInputPipeline(message.Text);

                var keyboardMarkup = new InlineKeyboardMarkup(new[]
                {
                    new[]
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)),
                    }
                });
                var replyMessage = hasError ? "Operation failed" : "Operation have processed successfully";

                telegramContext.MessageId = 0;
                return new Tuple<string, InlineKeyboardMarkup>(replyMessage, keyboardMarkup);
            }

            if (message.Text.Equals(nameof(Triggers.Start), StringComparison.OrdinalIgnoreCase))
            {
                telegramContext.MessageId = 0;

                if (userContext.State != States.Start)
                {
                    userContext.CatchEvent(Triggers.Start);
                }

                var handler = handlers.FirstOrDefault(h => h.Trigger == Triggers.Start);
                var reply = handler?.Handle(message.Text);

                if (!string.IsNullOrEmpty(reply?.Item1))
                {
                    return new Tuple<string, InlineKeyboardMarkup>(reply.Item1, reply.Item2);
                }
            }
        }

        return new Tuple<string, InlineKeyboardMarkup>(string.Empty, null);
    }
}