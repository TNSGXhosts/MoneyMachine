namespace Trading.Application.Core.APIRequestsEntities;
// TODO : Refactor file.
public class Session
{
    public string ClientId { get;set; }
    public string AccountId { get;set;}
    public int TimezoneOffset { get;set; }
    public string Locale { get;set; }
    public string Currency { get;set; }
    public string StreamEndpoint { get;set; }
}