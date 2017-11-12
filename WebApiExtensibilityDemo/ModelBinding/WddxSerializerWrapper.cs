using System;
using System.Linq;
using System.Reflection;
using Mueller.Wddx;

namespace WebApiExtensibilityDemo.ModelBinding
{
	public class WddxSerializerWrapper : IWddxSerializer
	{
		private readonly WddxSerializer serializer;
		private readonly WddxDeserializer deserializer;

		private readonly Lazy<MethodInfo> genericDeserializeMethod = new Lazy<MethodInfo>(FindGenericDeserializeMethod);

		public WddxSerializerWrapper()
		{
			serializer = new WddxSerializer();
			deserializer = new WddxDeserializer();

			var methods = typeof(WddxDeserializer)
				.GetMethods()
				.Where(m => m.Name == nameof(WddxDeserializer.Deserialize))
				.Where(m => m.IsGenericMethod)
				.Where(m => m.GetParameters().Length == 1 && m.GetParameters().Single().ParameterType == typeof(String))
				.ToList();
			if (methods.Count != 1)
			{
				throw new InvalidOperationException();
			}
		}

		public string Serialize(object value)
		{
			return serializer.Serialize(value);
		}

		public object Deserialize(string data, Type type)
		{
			//	WddxDeserializer.Deserialize(string WddxPacket) does not work correctly (it deserialized into array of values instead of one object).
			//	That's why we have to call Deserialize<T>(string WddxPacket) through reflection.

			var deserializeMethod = genericDeserializeMethod.Value.MakeGenericMethod(type);
			return deserializeMethod.Invoke(deserializer, new object[] { data });
		}

		private static MethodInfo FindGenericDeserializeMethod()
		{
			var methods = typeof(WddxDeserializer)
				.GetMethods()
				.Where(m => m.Name == nameof(WddxDeserializer.Deserialize))
				.Where(m => m.IsGenericMethod)
				.Where(m => m.GetParameters().Length == 1 && m.GetParameters().Single().ParameterType == typeof(String))
				.ToList();

			if (methods.Count == 0)
			{
				throw new InvalidOperationException("Failed to find generic WddxDeserializer.Deserialize() method");
			}

			if (methods.Count > 1)
			{
				throw new InvalidOperationException("Ambiguous WddxDeserializer.Deserialize() method");
			}

			return methods.Single();
		}
	}
}
