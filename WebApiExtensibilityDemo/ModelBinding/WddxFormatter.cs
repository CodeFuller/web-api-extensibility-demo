using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApiExtensibilityDemo.ModelBinding
{
	public class WddxFormatter : MediaTypeFormatter
	{
		private readonly IWddxSerializer serializer;

		public WddxFormatter(IWddxSerializer serializer)
		{
			this.serializer = serializer;

			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/wddx"));
		}

		public override bool CanReadType(Type type)
		{
			return true;
		}

		public override bool CanWriteType(Type type)
		{
			return true;
		}

		public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
		{
			string serializedData = serializer.Serialize(value);
			using (var writer = new StreamWriter(writeStream))
			{
				await writer.WriteAsync(serializedData);
			}
		}

		public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			using (var reader = new StreamReader(readStream))
			{
				var serializedData = await reader.ReadToEndAsync();
				return serializer.Deserialize(serializedData, type);
			}
		}
	}
}
