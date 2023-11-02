namespace Trading.Application.Configuration;

public sealed class TelegramSettings
{
    public const string ConfigurationSectionName = "TelegramSettings";

    public required string AccessToken { get; set; } = null!;
}
