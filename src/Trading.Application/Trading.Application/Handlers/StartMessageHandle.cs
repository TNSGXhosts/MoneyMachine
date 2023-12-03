using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class StartMessageHandler : IHandler
{
    public States State => States.Start;

    public Tuple<string, InlineKeyboardMarkup> HandleCallBack(string userInput)
    {
        return GetReply();
    }

    public Tuple<string, InlineKeyboardMarkup> HandleCallBackSelect(string userInput)
    {
        throw new NotImplementedException();
    }

    public Tuple<string, InlineKeyboardMarkup> HandleMessage(string userInput)
    {
        if (userInput != nameof(Triggers.Start))
        {
            return new Tuple<string, InlineKeyboardMarkup>("", null);
        }

        return GetReply();
    }

    private Tuple<string, InlineKeyboardMarkup> GetReply(){
        return new Tuple<string, InlineKeyboardMarkup>("Choose an action:", new InlineKeyboardMarkup(new[]
                {
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData("Create Order", nameof(Triggers.AddOrder)),
                        InlineKeyboardButton.WithCallbackData("Edit Order", nameof(Triggers.ChooseToUpdate)),
                        InlineKeyboardButton.WithCallbackData("Close Order", nameof(Triggers.ChooseToClose))
                    }
                }));
    }
}