namespace Trading.Application.Core.APIRequestsEntities;

public interface IHasErrorCode
{
    string ErrorCode { get; }
}

public class BaseResponse : IHasErrorCode
{
    public string DealReference { get; set; }

    public string ErrorCode { get; }
}
