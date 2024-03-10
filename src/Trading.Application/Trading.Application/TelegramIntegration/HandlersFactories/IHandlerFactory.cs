using Trading.Application.TelegramIntegration;

namespace Trading.Application;

public interface IHandlerFactory
{
    ICallbackHandler GetCallbackHandler();
    IMessageHandler GetMessageHandler();
}
