using System.Web.Http.Controllers;
using WebApiExtensibilityDemo.Helpers;

namespace WebApiExtensibilityDemo.ActionSelectors
{
	/// <summary>
	/// Custom Action Selector that uses actions stubs if request contains specific test HTTP header.
	/// </summary>
	public class StubHttpActionSelector : ApiControllerActionSelector
	{
		public override HttpActionDescriptor SelectAction(HttpControllerContext controllerContext)
		{
			HttpActionDescriptor normalAction = base.SelectAction(controllerContext);

			var isCalledFromTest = controllerContext.Request.TryGetHeaderValue("Called-From-Test") != null;
			if (!isCalledFromTest)
			{
				return normalAction;
			}

			var reflectedAction = normalAction as ReflectedHttpActionDescriptor;
			if (reflectedAction == null)
			{
				return normalAction;
			}

			var stubMethodInfo = controllerContext.Controller.GetType().GetMethod(reflectedAction.MethodInfo.Name + "Stub");
			if (stubMethodInfo != null)
			{
				reflectedAction.MethodInfo = stubMethodInfo;
			}

			return reflectedAction;
		}
	}
}
