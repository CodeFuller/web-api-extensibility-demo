using System;
using System.Web.Http.Dispatcher;
using WebApiExtensibilityDemo.Common;

namespace WebApiExtensibilityDemo.ControllerResolvers
{
	/// <summary>
	/// Custom controller type selector that takes only Controllers derived from IPluginHttpController.
	/// </summary>
	public class PluginHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
	{
		public PluginHttpControllerTypeResolver() :
			base(IsPluginHttpControllerType)
		{
		}

		private static bool IsPluginHttpControllerType(Type t)
		{
			return t != null &&
				t.IsClass &&
				t.IsVisible &&
				!t.IsAbstract &&
				typeof(IPluginHttpController).IsAssignableFrom(t);
		}
	}
}
