using System;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiExtensibilityDemo.AuthorizationFilters
{
	/// <summary>
	/// Example of Authorization filter based on AuthorizeAttribute.
	/// Checks whether the user role allows to perform requested content operation.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public sealed class ContentAuthorizeAttribute : AuthorizeAttribute
	{
		protected override bool IsAuthorized(HttpActionContext actionContext)
		{
			var method = actionContext.Request.Method;
			string requiredRole = (method == HttpMethod.Post || method == HttpMethod.Put || method == HttpMethod.Delete)
				? "ContentEditor"
				: "ContentUser";

			IPrincipal user = actionContext.RequestContext.Principal;
			return user != null && user.IsInRole(requiredRole);
		}
	}
}
