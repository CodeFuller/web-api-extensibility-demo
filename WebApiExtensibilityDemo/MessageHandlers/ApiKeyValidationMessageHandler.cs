using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApiExtensibilityDemo.Helpers;

namespace WebApiExtensibilityDemo.MessageHandlers
{
	/// <summary>
	/// Custom Message Handler that performs API key validation.
	/// </summary>
	public class ApiKeyValidationMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			string apiKey = request.TryGetHeaderValue("ApiKey");
			if (!ApiKeyIsValid(apiKey))
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
