namespace Trading.Application.TelegramIntegration;

public interface ITelegramClient
{
    Task RunAsync(CancellationTokenSource cts);

    Task SendMessageAsync(string message);
}
