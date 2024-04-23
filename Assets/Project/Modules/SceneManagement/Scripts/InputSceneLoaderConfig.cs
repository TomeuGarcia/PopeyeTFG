using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;

namespace Popeye.Modules.SceneManagement.Scripts
{
    [CreateAssetMenu(fileName = "InputSceneLoaderConfig", 
        menuName = ScriptableObjectsHelper.SCENELOADING_ASSETS_PATH + "InputSceneLoaderConfig")]
    public class InputSceneLoaderConfig : ScriptableObject
    {
        [System.Serializable]
        public class SceneLoadData
        {
            [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _sceneLoadGroup;
            [SerializeField] private KeyCode _loadKeyCode;

            public ISceneLoadManager.SceneAdditiveLoadGroup SceneLoadGroup => _sceneLoadGroup;
            public KeyCode LoadKeyCode => _loadKeyCode;
        }

        
        [Header("SCENES")]
        [SerializeField] private SceneLoadData[] _scenesData;
        public SceneLoadData[] ScenesData => _scenesData;
        
        
        [Header("RELOAD CURRENT SCENE")]
        [SerializeField] private SceneLoadOptionsAsset _reloadCurrentLoadOptions;
        [SerializeField] private KeyCode _reloadCurrentSceneKeyCode = KeyCode.R;
        public SceneLoadOptionsAsset ReloadCurrentLoadOptions => _reloadCurrentLoadOptions;
        public KeyCode ReloadCurrentSceneKeyCode => _reloadCurrentSceneKeyCode;
        
    }
}