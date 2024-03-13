using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.GameState.GaneralGameState
{
    
    [CreateAssetMenu(fileName = "GeneralGameStateData", 
        menuName = ScriptableObjectsHelper.GAMESTATE_ASSETS_PATH + "GeneralGameStateData")]
    public class GeneralGameStateData : ScriptableObject
    {
        [Header("FlAGS")] 
        [SerializeField] private bool _isTutorial;
        [SerializeField] private int _startingPowerBoostExperience = 0;

        public bool IsTutorial => _isTutorial;
        public int StartingPowerBoostExperience => _startingPowerBoostExperience;
    }
}