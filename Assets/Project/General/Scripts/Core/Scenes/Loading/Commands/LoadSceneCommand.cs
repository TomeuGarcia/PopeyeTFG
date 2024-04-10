using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class LoadSceneCommand : ISceneLoadCommand
    {
        private readonly ISceneReference _sceneReference;
        private readonly float _delay;

        public bool FinishedLoading { get; private set; }
        
        public LoadSceneCommand(ISceneReference sceneReference, float delay)
        {
            _sceneReference = sceneReference;
            _delay = delay;
            FinishedLoading = false;
        }

        public async UniTask Execute()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_delay));
            
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(_sceneReference.SceneName, LoadSceneMode.Single);
            
            await UniTask.WaitUntil(
                () => loadOperation.isDone
            );
            
            FinishedLoading = true;
        }
        
    }
}