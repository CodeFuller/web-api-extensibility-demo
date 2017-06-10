using System.Web.Http;

namespace WebApiExtensibilityDemo.Controllers
{
	public class TestController : ApiController
	{
		public string Get()
		{
			return "Some test content";
		}
	}
}
