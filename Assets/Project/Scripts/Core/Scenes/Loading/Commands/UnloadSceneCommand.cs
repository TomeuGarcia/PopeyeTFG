using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class UnloadSceneCommand : ICommand
    {
        private readonly string _sceneName;
        
        public UnloadSceneCommand(string sceneName)
        {
            _sceneName = sceneName;
        }
        

        
        public async UniTask Execute()
        {
            SceneManager.UnloadSceneAsync(_sceneName);
        }
        
    }
}