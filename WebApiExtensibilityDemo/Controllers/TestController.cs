using System.Web.Http;

namespace WebApiExtensibilityDemo.Controllers
{
	public class TestController : ApiController
	{
		public string GetContent()
		{
			return "Some test content";
		}

		[HttpPost]
		public void AddContent(string content)
		{
		}

		[NonAction]
		public string GetContentStub()
		{
			return "Some STUB content";
		}
	}
}
