using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.MessageHandlers
{
	/// <summary>
	/// Custom Message Handler that returns response directly without calling innder handler.
	/// </summary>
	public class StubMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			//	Returning response without actual processing
			return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
			{
				Content = new StringContent("Hello!")
			});
		}
	}
}
