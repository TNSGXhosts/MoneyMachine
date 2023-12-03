using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class ChooseOrderToClose : IHandler
{
    public States State => States.ChooseOrderToClose;

    public Tuple<string, InlineKeyboardMarkup> HandleCallBack(string userInput)
    {
        //TODO:get from API
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

        return new Tuple<string, InlineKeyboardMarkup>("Choose order to close:", markup);
    }

    public Tuple<string, InlineKeyboardMarkup> HandleCallBackSelect(string userInput)
    {
        //TODO: delete order on BL

        return new Tuple<string, InlineKeyboardMarkup>(
            "Operation result",
            new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(nameof(Triggers.Start))));
    }

    public Tuple<string, InlineKeyboardMarkup> HandleMessage(string userInput)
    {
        throw new NotImplementedException();
    }
}