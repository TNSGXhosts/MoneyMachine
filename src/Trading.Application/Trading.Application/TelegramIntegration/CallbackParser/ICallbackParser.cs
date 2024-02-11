using Trading.Application.TelegramConstants;

namespace Trading.Application.TelegramIntegration;

public interface ICallbackParser
{
    Tuple<Triggers, string>? ParseCallbackData(string callbackData);
}