using Trading.Application.TelegramConstants;

namespace Trading.Application.TelegramIntegration;

public class CallbackParser : ICallbackParser
{
    public Tuple<Triggers, string>? ParseCallbackData(string callbackData)
    {
        const int parametersMaxLength = 2;
        var parameters = callbackData.Split(ParserConstants.ParserConstants.Separator);

        if (Enum.TryParse(parameters[0], true, out Triggers parsedTrigger) && parameters.Length <= parametersMaxLength)
        {
            return new Tuple<Triggers, string>(parsedTrigger, parameters.Length > 1 ? parameters[1] : string.Empty);
        }

        return null;
    }
}