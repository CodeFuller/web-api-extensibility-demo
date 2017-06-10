using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using WebApiExtensibilityDemo.MessageHandlers;

namespace WebApiExtensibilityDemo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			//	Registering a Message Handler
			config.MessageHandlers.Add(new ApiKeyValidationMessageHandler());

			// Web API routes
			config.MapHttpAttributeRoutes();

			//	Registering Per-Route Message Handler
			//DelegatingHandler[] handlers =
			//{
			//	new ApiKeyValidationMessageHandler()
			//};
			//var routeHandlers = HttpClientFactory.CreatePipeline(new HttpControllerDispatcher(config), handlers);

			//config.Routes.MapHttpRoute(
			//	name: "ApiV2",
			//	routeTemplate: "api/v2/{controller}/{id}",
			//	defaults: new { id = RouteParameter.Optional },
			//	constraints: null,
			//	handler: routeHandlers
			//);

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}
