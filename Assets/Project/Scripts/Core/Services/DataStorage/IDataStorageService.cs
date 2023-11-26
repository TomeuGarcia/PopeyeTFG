namespace Popeye.Core.Services.DataStorage
{
	public interface IDataStorageService
	{
		void SetData<T>(T data, string name);
		T GetData<T>(string name);
	}
}