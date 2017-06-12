using System.Configuration;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using WebApiExtensibilityDemo.ControllerResolvers;
using WebApiExtensibilityDemo.MessageHandlers;

namespace WebApiExtensibilityDemo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			//ConfigureMessageHandlers(config);
			ConfigureControllerSelectors(config);

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}

		private static void ConfigureMessageHandlers(HttpConfiguration config)
		{
			config.MessageHandlers.Add(new ApiKeyValidationMessageHandler());

			//Registering Per-Route Message Handler
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
		}

		private static void ConfigureControllerSelectors(HttpConfiguration config)
		{
			config.Services.Replace(typeof(IAssembliesResolver), new PluginAssembliesResolver(ConfigurationManager.AppSettings["PluginsDirectory"]));
			config.Services.Replace(typeof(IHttpControllerTypeResolver), new PluginHttpControllerTypeResolver());
			config.Services.Replace(typeof(IHttpControllerSelector), new PluginHttpControllerSelector(
				config,
				new PluginAssembliesResolver(ConfigurationManager.AppSettings["PluginsDirectory"]),
				new PluginHttpControllerTypeResolver()));
			//config.Services.Replace(typeof(IHttpControllerSelector), new DynamicHttpControllerSelector(config));
		}
	}
}
