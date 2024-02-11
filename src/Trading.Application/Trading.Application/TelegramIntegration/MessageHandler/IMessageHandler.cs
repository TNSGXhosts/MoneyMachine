using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

public interface IMessageHandler
{
    Tuple<string, InlineKeyboardMarkup> HandleMessage(Message message);
}