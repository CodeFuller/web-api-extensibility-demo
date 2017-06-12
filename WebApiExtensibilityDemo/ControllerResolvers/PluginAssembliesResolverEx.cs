using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace WebApiExtensibilityDemo.ControllerResolvers
{
	/// <summary>
	/// Custom assemblies resolver that extends default list with assemblies from specified plugin directory.
	/// </summary>
	public class PluginAssembliesResolverEx : DefaultAssembliesResolver
	{
		private readonly string pluginDirectory;

		public PluginAssembliesResolverEx(string pluginDirectory)
		{
			this.pluginDirectory = pluginDirectory;
		}

		public override ICollection<Assembly> GetAssemblies()
		{
			var defaultAssemblies = base.GetAssemblies();
			var pluginAssemblies = Directory.GetFiles(pluginDirectory, "*.dll").Select(Assembly.LoadFrom);

			return defaultAssemblies.Concat(pluginAssemblies).ToList();
		}
	}
}
