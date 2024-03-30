using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Popeye.Modules.GameState.GaneralGameState
{
    
    [CreateAssetMenu(fileName = "GeneralGameStateData", 
        menuName = ScriptableObjectsHelper.GAMESTATE_ASSETS_PATH + "GeneralGameStateData")]
    public class GeneralGameStateData : ScriptableObject
    {
        [Header("FlAGS")] 
        [Scene] [SerializeField] private int _tutorialScene;
        [SerializeField] private int _startingPowerBoostExperience = 0;

        public bool IsTutorial => _tutorialScene == SceneManager.GetActiveScene().buildIndex;
        public int StartingPowerBoostExperience => _startingPowerBoostExperience;
    }
}