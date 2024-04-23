using Popeye.Core.Services.Serializer;
using UnityEngine;

namespace Popeye.Core.Services.DataStorage
{
	public class PlayerPrefsDataStorageService : IDataStorageService
	{
		private readonly ISerializerService _serializer;

		public PlayerPrefsDataStorageService(ISerializerService serializer)
		{
			_serializer = serializer;
		}

		public void SetData<T>(T data, string name)
		{
			PlayerPrefs.SetString(name, _serializer.Serialize(data));
			PlayerPrefs.Save();
		}

		public T GetData<T>(string name)
		{
			return _serializer.Deserialize<T>(PlayerPrefs.GetString(name));
		}
	}
}