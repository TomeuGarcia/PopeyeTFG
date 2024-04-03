using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class UnloadSceneCommand : ICommand
    {
        private readonly int _builtInSceneIndex;
        
        public UnloadSceneCommand(string sceneName)
        : this(SceneManager.GetSceneByName(sceneName).buildIndex)
        {
        }
        
        public UnloadSceneCommand(int builtInSceneIndex)
        {
            _builtInSceneIndex = builtInSceneIndex;
        }
        
        public async UniTask Execute()
        {
            await UniTask.WaitUntil(
                () => SceneManager.UnloadSceneAsync(_builtInSceneIndex).isDone
            );
        }
        
    }
}