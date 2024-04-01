using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class LoadSceneAdditivelyCommand : ICommand
    {
        private readonly int _builtInSceneIndex;
        
        public bool FinishedLoading { get; private set; }
        
        public LoadSceneAdditivelyCommand(string sceneName) : 
            this(SceneManager.GetSceneByName(sceneName).buildIndex)
        {
        }
        public LoadSceneAdditivelyCommand(int builtInSceneIndex)
        {
            _builtInSceneIndex = builtInSceneIndex;
            FinishedLoading = false;
        }
        
        public async UniTask Execute()
        {
            await UniTask.WaitUntil(
                () => SceneManager.LoadSceneAsync(_builtInSceneIndex, LoadSceneMode.Additive).isDone
            );
            
            FinishedLoading = true;
        }
        
    }
}