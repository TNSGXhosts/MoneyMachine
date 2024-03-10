using Microsoft.Extensions.DependencyInjection;

using Trading.Application.TelegramIntegration;

namespace Trading.Application;

public class HandlerFactory(IServiceProvider serviceProvider) : IHandlerFactory
{
    public ICallbackHandler GetCallbackHandler()
    {
        return serviceProvider.GetRequiredService<ICallbackHandler>();
    }

    public IMessageHandler GetMessageHandler()
    {
        return serviceProvider.GetRequiredService<IMessageHandler>();
    }
}
