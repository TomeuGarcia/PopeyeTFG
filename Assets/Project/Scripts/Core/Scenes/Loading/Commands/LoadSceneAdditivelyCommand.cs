using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class LoadSceneAdditivelyCommand : ISceneLoadCommand
    {
        private readonly string _sceneName;
        private readonly float _delay;

        public bool FinishedLoading { get; private set; }
        
        public LoadSceneAdditivelyCommand(string sceneName, float delay)
        {
            _sceneName = sceneName;
            _delay = delay;
            FinishedLoading = false;
        }

        
        public async UniTask Execute()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));

            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);
            
            await UniTask.WaitUntil(
                () => loadOperation.isDone
            );
            
            FinishedLoading = true;
        }
        
    }
}