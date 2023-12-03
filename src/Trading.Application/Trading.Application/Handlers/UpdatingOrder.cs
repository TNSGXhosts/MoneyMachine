using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class UpdatingOrderHandler : IHandler
{
    public States State => States.UpdatingOrder;

    private string _callbackInput = string.Empty;

    public Tuple<string, InlineKeyboardMarkup> HandleCallBack(string userInput)
    {
        //TODO:Get orders from BL

        var markup = new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("USDCHF", "id123"),
                        InlineKeyboardButton.WithCallbackData("EURUSD", "id321"),
                        InlineKeyboardButton.WithCallbackData("USDMXN", "id412")
                    },
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start))
                    }
                });

        return new Tuple<string, InlineKeyboardMarkup>("Choose order to update",markup);
    }

    public Tuple<string, InlineKeyboardMarkup> HandleMessage(string userInput)
    {
        var parameters = string.Concat(_callbackInput, Environment.NewLine, userInput);
        //TODO:Update Order through BL
        _callbackInput = string.Empty;

        return new Tuple<string, InlineKeyboardMarkup>(
            "Operation result",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(nameof(Triggers.Start))));
    }

    public Tuple<string, InlineKeyboardMarkup> HandleCallBackSelect(string userInput)
    {
        _callbackInput = userInput;

        return new Tuple<string, InlineKeyboardMarkup>(
            "Enter new order info: price, takeprofit, stoploss",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(nameof(Triggers.Start))));
    }
}