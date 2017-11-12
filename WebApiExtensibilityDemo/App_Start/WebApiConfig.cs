using System.Configuration;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using WebApiExtensibilityDemo.ActionSelectors;
using WebApiExtensibilityDemo.AuthenticationFilters;
using WebApiExtensibilityDemo.AuthorizationFilters;
using WebApiExtensibilityDemo.ControllerActivators;
using WebApiExtensibilityDemo.ControllerResolvers;
using WebApiExtensibilityDemo.MessageHandlers;
using WebApiExtensibilityDemo.ModelBinding;

namespace WebApiExtensibilityDemo
{
	public static class WebApiConfig
	{
		private static IUnityContainer diContainer;

		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			ConfigureUnityContainer();

			//ConfigureMessageHandlers(config);
			//ConfigureControllerSelectors(config);
			//ConfigureControllerActivator(config);
			//ConfigureActionSelector(config);
			//ConfigureFilters(config);

			ConfigureModelBinding(config);

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
		}

		private static void ConfigureUnityContainer()
		{
			diContainer = new UnityContainer();

			//	Put any dependency registrations here

			diContainer.RegisterType<IAuthorizationFilter, SubscriptionAuthorizeFilter>();
			diContainer.RegisterType<ISubscriptionRepository, TestSubscriptionRepository>();
			diContainer.RegisterType<IWddxSerializer, WddxSerializerWrapper>();
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

		private static void ConfigureControllerActivator(HttpConfiguration config)
		{
			config.Services.Replace(typeof(IHttpControllerActivator), new UnityHttpControllerActivator(diContainer));
		}

		private static void ConfigureActionSelector(HttpConfiguration config)
		{
			config.Services.Replace(typeof(IHttpActionSelector), new StubHttpActionSelector());
		}

		private static void ConfigureFilters(HttpConfiguration config)
		{
			config.Filters.Add(new BasicAuthenticationFilterAttribute());
			//config.Filters.Add(new ContentAuthorizeAttribute());
			//config.Filters.Add(new SubscriptionAuthorizeAttribute());
			config.Filters.Add(diContainer.Resolve<IAuthorizationFilter>());
		}

		private static void ConfigureModelBinding(HttpConfiguration config)
		{
			config.Formatters.Add(diContainer.Resolve<WddxFormatter>());
		}
	}
}
