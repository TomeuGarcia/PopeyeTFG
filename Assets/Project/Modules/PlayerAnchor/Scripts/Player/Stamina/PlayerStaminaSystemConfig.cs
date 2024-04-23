using Popeye.Modules.ValueStatSystem;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.Stamina
{
    
    [CreateAssetMenu(fileName = "PlayerStaminaSystemConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerStaminaSystemConfig")]
    public class PlayerStaminaSystemConfig : ScriptableObject
    {
        [SerializeField] private TimeStepsStaminaSystemConfig _baseStaminaConfig;
        [SerializeField] private TimeStepsStaminaSystemConfig _extraStaminaConfig;
        
        
        public TimeStepsStaminaSystemConfig BaseStaminaConfig => _baseStaminaConfig;
        public TimeStepsStaminaSystemConfig ExtraStaminaConfig => _extraStaminaConfig;
    }
}