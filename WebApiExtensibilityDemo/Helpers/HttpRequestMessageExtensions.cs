using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace WebApiExtensibilityDemo.Helpers
{
	public static class HttpRequestMessageExtensions
	{
		public static string GetHeaderValue(this HttpRequestMessage request, string headerName)
		{
			var value = request.TryGetHeaderValue(headerName);
			if (value == null)
			{
				throw new InvalidOperationException($"Header '{headerName}' is not defined");
			}

			return value;
		}

		public static string TryGetHeaderValue(this HttpRequestMessage request, string headerName)
		{
			IEnumerable<string> values;
			return request.Headers.TryGetValues(headerName, out values) ? values.FirstOrDefault() : null;
		}
	}
}
