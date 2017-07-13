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
	/// Example of Authorization filter based on AuthorizationFilterAttribute.
	/// Checks whether the user has active API subscription.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public sealed class SubscriptionAuthorizeAttribute : AuthorizationFilterAttribute
	{
		public ISubscriptionRepository SubscriptionRepository { get; set; } = new TestSubscriptionRepository();

		public override async Task OnAuthorizationAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
		{
			IPrincipal user = actionContext.RequestContext.Principal;
			if (user == null || !(await SubscriptionRepository.CheckForActiveSubscription(user.Identity.Name, cancellationToken)))
			{
				actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Request not authorized");
			}
		}
	}
}
