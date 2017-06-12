using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace WebApiExtensibilityDemo.ControllerResolvers
{
	/// <summary>
	/// Custom controller selector that allows adding controller plugins to running application.
	/// </summary>
	public class PluginHttpControllerSelector : DefaultHttpControllerSelector
	{
		private readonly HttpConfiguration configuration;
		private readonly IAssembliesResolver assembliesResolver;
		private readonly IHttpControllerTypeResolver controllerTypeResolver;

		public PluginHttpControllerSelector(HttpConfiguration configuration, IAssembliesResolver assembliesResolver, IHttpControllerTypeResolver controllerTypeResolver) :
			base(configuration)
		{
			this.configuration = configuration;
			this.assembliesResolver = assembliesResolver;
			this.controllerTypeResolver = controllerTypeResolver;
		}

		public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			var controllerName = GetControllerName(request);
			var controllerTypeName = $"{controllerName}Controller".ToUpper();

			var controllerType = controllerTypeResolver.GetControllerTypes(assembliesResolver).
				SingleOrDefault(t => t.Name.ToUpper() == controllerTypeName);
			if (controllerType == null)
			{
				throw new HttpResponseException(request.CreateErrorResponse(
					HttpStatusCode.NotFound, $"Unknown controller '{controllerName}'"));
			}

			return new HttpControllerDescriptor(configuration, controllerTypeName, controllerType);
		}

		public override IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
		{
			return null;
		}
	}
}
