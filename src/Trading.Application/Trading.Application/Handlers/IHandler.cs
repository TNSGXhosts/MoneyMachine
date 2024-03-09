using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

public interface IHandler
{
    Triggers Trigger { get; }

    Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput);
}