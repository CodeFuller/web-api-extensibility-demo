using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using WebApiExtensibilityDemo.Helpers;

namespace WebApiExtensibilityDemo.ControllerResolvers
{
	/// <summary>
	/// Custom controller selector that chooses target controller based on the data the request.
	/// </summary>
	public class DynamicHttpControllerSelector : IHttpControllerSelector
	{
		private readonly HttpConfiguration configuration;

		public DynamicHttpControllerSelector(HttpConfiguration configuration)
		{
			this.configuration = configuration;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.Reflection.Assembly.LoadFile", Justification = "The method is required by requested functionality")]
		public HttpControllerDescriptor SelectController(HttpRequestMessage request)
		{
			var assemblyPath = request.GetHeaderValue("ControllerAssemblyPath");
			var controllerTypeName = request.GetHeaderValue("ControllerTypeName");

			var assembly = Assembly.LoadFile(assemblyPath);
			var controllerType = assembly.GetType(controllerTypeName);

			return new HttpControllerDescriptor(configuration, controllerTypeName, controllerType);
		}

		public IDictionary<string, HttpControllerDescriptor> GetControllerMapping()
		{
			return null;
		}
	}
}
