namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneLoadManager
    {
        [System.Serializable]
        public struct SceneAdditiveLoadGroup
        {
            public SceneReferenceAsset sceneReference;
            public SceneLoadOptionsAsset loadOptions;
        }

        public readonly struct OnStartLoadingAdditiveSceneEvent
        {
            private readonly ISceneReference _sceneReference;
            public ISceneReference SceneReference => _sceneReference;

            public OnStartLoadingAdditiveSceneEvent(ISceneReference sceneReference)
            {
                _sceneReference = sceneReference;
            }
        }
        public readonly struct OnStartUnloadingSceneEvent
        {
            private readonly ISceneReference _sceneReference;
            public ISceneReference SceneReference => _sceneReference;
            
            public OnStartUnloadingSceneEvent(ISceneReference sceneReference)
            {
                _sceneReference = sceneReference;
            }
        }
        
        
        void LoadSceneAdditively(SceneAdditiveLoadGroup sceneLoadGroup);
        void LoadScene(SceneAdditiveLoadGroup sceneLoadGroup);
        void UnloadScene(SceneReferenceAsset sceneReference);
        void ReloadCurrentScene(SceneLoadOptionsAsset loadOptions);
        
    }
}