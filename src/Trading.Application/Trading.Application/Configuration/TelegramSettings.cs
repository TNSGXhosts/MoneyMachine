namespace Trading.Application.Configuration;

public sealed class TelegramSettings
{
    public required string AccessToken { get; set; } = null!;

    public required string ChatId { get; set; } = null!;
}
