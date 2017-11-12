using System;

namespace WebApiExtensibilityDemo.ModelBinding
{
	public interface IWddxSerializer
	{
		string Serialize(Object value);

		object Deserialize(string data, Type type);
	}
}
