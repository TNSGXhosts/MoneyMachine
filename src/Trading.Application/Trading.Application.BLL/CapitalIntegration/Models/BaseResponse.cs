namespace Trading.Application.BLL.CapitalIntegration.Models;

internal interface IHasErrorCode
{
    string ErrorCode { get; }
}

internal class BaseResponse : IHasErrorCode
{
    public string DealReference { get; set; }

    public string ErrorCode { get; }
}
