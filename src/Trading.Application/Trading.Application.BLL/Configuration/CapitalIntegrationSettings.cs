namespace Trading.Application.BLL.Configuration;

public sealed class CapitalIntegrationSettings
{
    public required string ApiKey { get; init; } = string.Empty;

    public required string Identifier { get; init; } = string.Empty;

    public required string Password { get; init; } = string.Empty;

    public required string BaseUrl { get; init; } = string.Empty;
}
