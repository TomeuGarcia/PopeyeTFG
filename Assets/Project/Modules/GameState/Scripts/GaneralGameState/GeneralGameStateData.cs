using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.GameState.GaneralGameState
{
    
    [CreateAssetMenu(fileName = "GeneralGameStateData", 
        menuName = ScriptableObjectsHelper.GAMESTATE_ASSETS_PATH + "GeneralGameStateData")]
    public class GeneralGameStateData : ScriptableObject
    {
        [Header("FlAGS")] 
        [SerializeField] private bool _isTutorial;
        [SerializeField] private int _startingPowerBoosts = 0;

        public bool IsTutorial => _isTutorial;
        public int StartingPowerBoosts => _startingPowerBoosts;
    }
}