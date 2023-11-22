using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Commands
{
	public class LoadSceneAsyncCommand : ICommand
	{
		private readonly string _sceneToLoad;

		public LoadSceneAsyncCommand(string sceneToLoad)
		{
			_sceneToLoad = sceneToLoad;
		}
		
		public async UniTask Execute()
		{
			await SceneManager.LoadSceneAsync(_sceneToLoad);
		}
	}
}