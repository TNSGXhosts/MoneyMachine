using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Trading.Application.TelegramIntegration;

public interface ICallbackHandler
{
    Tuple<string, InlineKeyboardMarkup> HandleCallback(CallbackQuery callbackQuery);
}