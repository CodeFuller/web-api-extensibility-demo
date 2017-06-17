using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Practices.Unity;

namespace WebApiExtensibilityDemo.ControllerActivators
{
	/// <summary>
	/// Unity based controller activator.
	/// </summary>
	public class UnityHttpControllerActivator : IHttpControllerActivator
	{
		private readonly IUnityContainer unityContainer;

		public UnityHttpControllerActivator(IUnityContainer unityContainer)
		{
			this.unityContainer = unityContainer;
		}

		public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
		{
			return (IHttpController)unityContainer.Resolve(controllerType);
		}
	}
}
