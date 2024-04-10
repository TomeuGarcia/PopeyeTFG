using System;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.CommandQueue;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    public class UnloadSceneCommand : ICommand
    {
        private readonly ISceneReference _sceneReference;        
        private readonly Action<ISceneReference> _startUnloadingCallback;
        
        public UnloadSceneCommand(ISceneReference sceneReference, Action<ISceneReference> startUnloadingCallback)
        {
            _sceneReference = sceneReference;
            _startUnloadingCallback = startUnloadingCallback;
        }
        

        
        public async UniTask Execute()
        {
            _startUnloadingCallback?.Invoke(_sceneReference);
            SceneManager.UnloadSceneAsync(_sceneReference.SceneName);
        }
        
    }
}