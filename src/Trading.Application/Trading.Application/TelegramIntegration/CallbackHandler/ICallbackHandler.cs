using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

public interface ICallbackHandler
{
    Task<Tuple<string, InlineKeyboardMarkup>> HandleCallback(CallbackQuery callbackQuery);
}