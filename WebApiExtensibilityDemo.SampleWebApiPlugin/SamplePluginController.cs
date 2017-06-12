using System.Web.Http;
using WebApiExtensibilityDemo.Common;

namespace WebApiExtensibilityDemo.SampleWebApiPlugin
{
	public class SamplePluginController : ApiController, IPluginHttpController
	{
		public string Get()
		{
			return "Some content from Plugin Controller";
		}
	}
}
