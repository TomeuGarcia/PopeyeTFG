using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Commands
{
	public class LoadSceneCommand : ICommand
	{
		private readonly string _sceneToLoad;
		private int _sceneLoadDelayMilliseconds;

		public LoadSceneCommand(string sceneToLoad, int sceneLoadDelayMilliseconds)
		{
			_sceneToLoad = sceneToLoad;
			_sceneLoadDelayMilliseconds = sceneLoadDelayMilliseconds;

        }
		
		public async UniTask Execute()
		{
			await UniTask.Delay(_sceneLoadDelayMilliseconds);
			SceneManager.LoadScene(_sceneToLoad);
		}
	}
}