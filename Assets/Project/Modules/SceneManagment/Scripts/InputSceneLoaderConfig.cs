using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.SceneManagment.Scripts
{
    [CreateAssetMenu(fileName = "InputSceneLoaderConfig", 
        menuName = ScriptableObjectsHelper.SCENELOADING_ASSETS_PATH + "InputSceneLoaderConfig")]
    public class InputSceneLoaderConfig : ScriptableObject
    {
        [System.Serializable]
        public class SceneLoadData
        {
            [SerializeField, Min(0)] private int _builtInSceneIndex;
            [SerializeField] private KeyCode _loadKeyCode;

            public int BuiltInSceneIndex => _builtInSceneIndex;
            public KeyCode LoadKeyCode => _loadKeyCode;
        }


        [SerializeField] private SceneLoadData[] _scenesData;
        public SceneLoadData[] ScenesData => _scenesData;
        
        
        [SerializeField] private KeyCode _reloadCurrentSceneKeyCode = KeyCode.R;
        public KeyCode ReloadCurrentSceneKeyCode => _reloadCurrentSceneKeyCode;

    }
}