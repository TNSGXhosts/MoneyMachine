using System;
using RestSharp;

namespace MoneyMachine.Extensions
{
	public static class RestResponseExtensions
	{
		public static void ProcessResponse(this RestResponse restResponse)
		{
			if (!restResponse.IsSuccessful)
			{
                throw new Exception(restResponse?.ErrorException?.Message);
            }
		}
	}
}

