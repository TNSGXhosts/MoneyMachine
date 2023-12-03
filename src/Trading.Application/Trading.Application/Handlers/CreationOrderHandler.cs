using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class CreationOrderHandler : IHandler
{
    public States State => States.CreationOrder;

    public Tuple<string, InlineKeyboardMarkup> HandleCallBack(string userInput)
    {
        return new Tuple<string, InlineKeyboardMarkup>(
            "Enter info: ticket, price, takeprofit, stoploss",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start))));
    }

    public Tuple<string, InlineKeyboardMarkup> HandleCallBackSelect(string userInput)
    {
        throw new NotImplementedException();
    }

    public Tuple<string, InlineKeyboardMarkup> HandleMessage(string userInput)
    {
        //TODO: create order in BL

        return new Tuple<string, InlineKeyboardMarkup>(
            "Operation result",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start))));
    }
}