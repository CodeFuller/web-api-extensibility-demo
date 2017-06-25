using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace WebApiExtensibilityDemo.AuthenticationFilters
{
	/// <summary>
	/// Authentication filter that performs Basic Authentication.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class BasicAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
	{
		private readonly IAuthenticationAgent authenticationAgent = new AuthenticationAgent();

		public bool AllowMultiple => false;

		public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			var request = context.Request;
			AuthenticationHeaderValue authorization = request.Headers.Authorization;
			// If request doesn't contain any credentials - return without setting 401 Unauthorized status code.
			if (authorization == null)
			{
				return;
			}

			// If authorization scheme differs from implemented by the filter -
			// return and give other authentication filter in pipeline to recognize and process it.
			if (!"Basic".Equals(authorization.Scheme, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}

			if (String.IsNullOrEmpty(authorization.Parameter))
			{
				context.ErrorResult = new UnauthorizedResult(Enumerable.Empty<AuthenticationHeaderValue>(), context.Request);
				return;
			}

			Tuple<string, string> userCredentials = ExtractUserCredentials(authorization.Parameter);
			if (userCredentials == null)
			{
				context.ErrorResult = new UnauthorizedResult(Enumerable.Empty<AuthenticationHeaderValue>(), context.Request);
				return;
			}

			var principal = await authenticationAgent.AuthenticateUser(userCredentials.Item1, userCredentials.Item2);
			if (principal == null)
			{
				context.ErrorResult = new UnauthorizedResult(Enumerable.Empty<AuthenticationHeaderValue>(), context.Request);
			}
			else
			{
				context.Principal = principal;
			}
		}

		public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
		{
			context.Result = new AddChallengeToHttpResponse(context.Result);
		}

		private static Tuple<string, string> ExtractUserCredentials(string base64Data)
		{
			byte[] decodedData;
			try
			{
				decodedData = Convert.FromBase64String(base64Data);
			}
			catch (FormatException)
			{
				return null;
			}
			string userCredentials = Encoding.GetEncoding("iso-8859-1").GetString(decodedData);

			int colonIndex = userCredentials.IndexOf(':');
			return colonIndex == -1 ? null : new Tuple<string, string>(userCredentials.Substring(0, colonIndex),
				userCredentials.Substring(colonIndex + 1));
		}
	}
}
