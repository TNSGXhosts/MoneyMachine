using Telegram.Bot.Types.ReplyMarkups;

using Trading.Application.BLL.CapitalIntegration;

using Trading.Application.TelegramConstants;

namespace Trading.Application.Handlers;

internal class ChoosePositionHandler(IPositionClient positionClient) : IHandler
{
    public Triggers Trigger => Triggers.ChoosePosition;

    public Task<Tuple<string, InlineKeyboardMarkup>> HandleAsync(string userInput)
    {
        var positions = positionClient.GetPositionsAsync();
        var keyboardButtons = positions.Result
            .Select(
                o => InlineKeyboardButton.WithCallbackData(
                    $"{o.Market.Epic}-{o.Position.Level}",
                    $"{nameof(Triggers.SelectPosition)}{ParserConstants.ParserConstants.Separator}{o.Position.DealId}"))
            .ToList();
        var buttonLines = new List<List<InlineKeyboardButton>>();
        for (var i = 0; i < keyboardButtons.Count; i += 4)
        {
            var subArray = keyboardButtons.GetRange(i, Math.Min(4, keyboardButtons.Count - i));
            buttonLines.Add(subArray);
        }

        buttonLines.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Go back", nameof(Triggers.Start)) });
        var keyboardMarkup = new InlineKeyboardMarkup(buttonLines);

        return Task.FromResult(new Tuple<string, InlineKeyboardMarkup>(
            "Choose a position:",
            keyboardMarkup));
    }
}