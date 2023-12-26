using UnityEngine;

namespace Popeye.Core.Services.Serializer
{
	public class UnityJsonSerializerService : ISerializerService
	{
		public string Serialize<T>(T data)
		{
			return JsonUtility.ToJson(data);
		}

		public T Deserialize<T>(string data)
		{
			return JsonUtility.FromJson<T>(data);
		}
	}
}