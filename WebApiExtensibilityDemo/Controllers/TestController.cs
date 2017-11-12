using System.Web.Http;
using WebApiExtensibilityDemo.Models;

namespace WebApiExtensibilityDemo.Controllers
{
	public class TestController : ApiController
	{
		public SomeData GetContent()
		{
			return new SomeData
			{
				StringField = "Some string",
				NumericField = 123,
			};
		}

		[HttpPost]
		public void AddContent(SomeData data)
		{
		}

		[NonAction]
		public SomeData GetContentStub()
		{
			return new SomeData
			{
				StringField = "Stub data",
				NumericField = 123,
			};
		}
	}
}
