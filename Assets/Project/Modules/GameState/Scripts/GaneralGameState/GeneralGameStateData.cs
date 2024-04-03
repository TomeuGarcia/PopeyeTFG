using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.AbilityUnlock;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Core.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Popeye.Modules.GameState.GaneralGameState
{
    
    [CreateAssetMenu(fileName = "GeneralGameStateData", 
        menuName = ScriptableObjectsHelper.GAMESTATE_ASSETS_PATH + "GeneralGameStateData")]
    public class GeneralGameStateData : ScriptableObject
    {
        [Header("FLAGS")] 
        [SerializeField] private SceneReferenceAsset _tutorialScene;
        [Expandable] [SerializeField] private PlayerUnlockableAbilitiesConfig _unlockableAbilitiesConfig;

        public bool IsTutorial => _tutorialScene.BuiltInSceneIndex == SceneManager.GetActiveScene().buildIndex;
        public PlayerUnlockableAbilitiesConfig PlayerUnlockableAbilitiesConfig => _unlockableAbilitiesConfig;


        public void LoadState()
        {
            _unlockableAbilitiesConfig.SetupState(IsTutorial);
        }
    }
}