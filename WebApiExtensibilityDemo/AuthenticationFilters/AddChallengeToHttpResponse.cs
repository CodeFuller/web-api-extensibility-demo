using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApiExtensibilityDemo.AuthenticationFilters
{
	internal class AddChallengeToHttpResponse : IHttpActionResult
	{
		private static string Scheme = "Basic";
		private static string Realm = "API realm";

		private readonly IHttpActionResult innerResult;

		public AddChallengeToHttpResponse(IHttpActionResult innerResult)
		{
			this.innerResult = innerResult;
		}

		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage response = await innerResult.ExecuteAsync(cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				//	Checking whether the challenge is not yet set for this authentication scheme.
				if (response.Headers.WwwAuthenticate.All(h => h.Scheme != Scheme))
				{
					response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Scheme, $"realm=\"{Realm}\""));
				}
			}

			return response;
		}
	}
}
