using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.MessageHandlers
{
	public class ApiKeyValidationMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			IEnumerable<string> apiKeyValues;
			if (!request.Headers.TryGetValues("ApiKey", out apiKeyValues) ||
				!ApiKeyIsValid(apiKeyValues.SingleOrDefault()))
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden)
				{
					Content = new StringContent("ApiKey is missing or invalid")
				};
			}

			return await base.SendAsync(request, cancellationToken);
		}

		public bool ApiKeyIsValid(string apiKey)
		{
			return apiKey?.Length == 5;
		}
	}
}
