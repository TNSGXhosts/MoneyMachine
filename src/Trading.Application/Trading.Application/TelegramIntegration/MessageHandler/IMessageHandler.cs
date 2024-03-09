using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

public interface IMessageHandler
{
    Task<Tuple<string, InlineKeyboardMarkup>> HandleMessage(Message message);
}