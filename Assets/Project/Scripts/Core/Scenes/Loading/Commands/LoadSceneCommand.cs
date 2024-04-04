using System;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class LoadSceneCommand : ISceneLoadCommand
    {
        private readonly int _builtInSceneIndex;
        private readonly float _delay;

        public bool FinishedLoading { get; private set; }
        
        public LoadSceneCommand(string sceneName, float delay) : 
            this(SceneManager.GetSceneByName(sceneName).buildIndex, delay)
        {
        }
        public LoadSceneCommand(int builtInSceneIndex, float delay)
        {
            _builtInSceneIndex = builtInSceneIndex;
            _delay = delay;
            FinishedLoading = false;
        }
        
        public async UniTask Execute()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            
            await UniTask.WaitUntil(
                () => SceneManager.LoadSceneAsync(_builtInSceneIndex, LoadSceneMode.Single).isDone
            );
            
            FinishedLoading = true;
        }
        
    }
}