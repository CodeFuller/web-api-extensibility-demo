using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.MessageHandlers
{
	public class TimingMessageHandler : DelegatingHandler
	{
		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			Stopwatch sw = Stopwatch.StartNew();
			var response = await base.SendAsync(request, cancellationToken);
			sw.Stop();

			response.Headers.Add("Timing", sw.ElapsedMilliseconds.ToString());
			return response;
		}
	}
}
