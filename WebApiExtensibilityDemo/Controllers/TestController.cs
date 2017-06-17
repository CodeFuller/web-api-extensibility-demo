using System.Web.Http;

namespace WebApiExtensibilityDemo.Controllers
{
	public class TestController : ApiController
	{
		public string GetContent()
		{
			return "Some test content";
		}

		[NonAction]
		public string GetContentStub()
		{
			return "Some STUB content";
		}
	}
}
