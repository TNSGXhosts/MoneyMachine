using System;
using MoneyMachine.Constants;
using RestSharp;

namespace MoneyMachine.Extensions
{
	public static class RestRequestExtensions
	{
		public static RestRequest SetCommonRequestData(this RestRequest request, string securityToken, string cst)
		{
            request.AddHeader(Constants.Headers.SecurityToken, securityToken);
            request.AddHeader(Constants.Headers.CST, cst);
			return request;
        }
	}
}

