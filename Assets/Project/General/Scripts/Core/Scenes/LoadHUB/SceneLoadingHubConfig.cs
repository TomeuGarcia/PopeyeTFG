using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Scripts.Core.Scenes
{
    [CreateAssetMenu(fileName = "SceneLoadingHubConfig", 
        menuName = ScriptableObjectsHelper.SCENES_ASSETS_PATH + "SceneLoadingHubConfig")]
    public class SceneLoadingHubConfig : ScriptableObject
    {
        [Header("MAIN MENU SCENE")]
        [SerializeField] private SceneReferenceAsset _mainMenuScene;
        
        [Header("GAMEPLAY SCENE")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup _gameplayCoreSceneGroup;
        
        [Header("SCENES")]
        [SerializeField] private ISceneLoadManager.SceneAdditiveLoadGroup[] _scenesToLoad;
        
        
        public SceneReferenceAsset MainMenuScene => _mainMenuScene;        
        public ISceneLoadManager.SceneAdditiveLoadGroup GameplayCoreSceneGroup => _gameplayCoreSceneGroup;        
        public ISceneLoadManager.SceneAdditiveLoadGroup[] ScenesToLoad => _scenesToLoad;

    }
}