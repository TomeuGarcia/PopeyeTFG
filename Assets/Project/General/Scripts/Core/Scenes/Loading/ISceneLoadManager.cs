using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    public interface ISceneLoadManager
    {
        [System.Serializable]
        public struct SceneAdditiveLoadGroup
        {
            [SerializeField] private SceneReferenceAsset _sceneReference;
            [SerializeField] private SceneLoadOptionsAsset _loadOptions;
            
            public ISceneReference SceneReference => _sceneReference;
            public SceneLoadOptions LoadOptions => _loadOptions.AdditiveSceneLoadOptions;

            public SceneAdditiveLoadGroup(SceneReferenceAsset sceneReference, SceneLoadOptionsAsset loadOptions)
            {
                _sceneReference = sceneReference;
                _loadOptions = loadOptions;
            }
        }
        

        void LoadSceneAdditively(SceneAdditiveLoadGroup sceneLoadGroup);
        void LoadScene(SceneAdditiveLoadGroup sceneLoadGroup);
        void ReloadCurrentScene(SceneLoadOptionsAsset loadOptions);
        
    }
}