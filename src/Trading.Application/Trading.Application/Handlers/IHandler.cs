using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

public interface IHandler
{
    States State { get; }

    Tuple<string, InlineKeyboardMarkup>HandleMessage(string userInput);
    Tuple<string, InlineKeyboardMarkup>HandleCallBack(string userInput);
    Tuple<string, InlineKeyboardMarkup>HandleCallBackSelect(string userInput);
}