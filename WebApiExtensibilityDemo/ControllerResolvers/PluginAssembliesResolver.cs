using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;

namespace WebApiExtensibilityDemo.ControllerResolvers
{
	/// <summary>
	/// Custom implementation of IAssembliesResolver that returns all assemblies from specified plugin directory.
	/// </summary>
	public class PluginAssembliesResolver : IAssembliesResolver
	{
		private readonly string pluginDirectory;

		public PluginAssembliesResolver(string pluginDirectory)
		{
			this.pluginDirectory = pluginDirectory;
		}

		public ICollection<Assembly> GetAssemblies()
		{
			return Directory.GetFiles(pluginDirectory, "*.dll").Select(Assembly.LoadFrom).ToList();
		}
	}
}
