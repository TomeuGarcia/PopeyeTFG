using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Scripts.Core.Scenes
{
    [CreateAssetMenu(fileName = "SceneReference_NAME", 
        menuName = ScriptableObjectsHelper.SCENES_ASSETS_PATH + "SceneReference")]
    public class SceneReferenceAsset : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        public string SceneName => _sceneName;

        
        
        
        
        public void SetSceneName(string sceneName)
        {
            _sceneName = sceneName;
        }
    }
}