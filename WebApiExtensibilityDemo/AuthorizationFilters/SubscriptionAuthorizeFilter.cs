using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace WebApiExtensibilityDemo.AuthorizationFilters
{
	/// <summary>
	/// Example of raw Authorization filter based on IAuthorizationFilter.
	/// Checks whether the user has active API subscription.
	/// </summary>
	public class SubscriptionAuthorizeFilter : IAuthorizationFilter
	{
		private readonly ISubscriptionRepository subscriptionRepository;

		public bool AllowMultiple => false;

		public SubscriptionAuthorizeFilter(ISubscriptionRepository subscriptionRepository)
		{
			if (subscriptionRepository == null)
			{
				throw new ArgumentNullException(nameof(subscriptionRepository));
			}

			this.subscriptionRepository = subscriptionRepository;
		}

		public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
			Func<Task<HttpResponseMessage>> continuation)
		{
			IPrincipal user = actionContext.RequestContext.Principal;
			if (user == null || !(await subscriptionRepository.CheckForActiveSubscription(user.Identity.Name, cancellationToken)))
			{
				HttpResponseMessage response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Request not authorized");
				actionContext.Response = response;
				return response;
			}

			return await continuation();
		}
	}
}
