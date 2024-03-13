using Popeye.Modules.PlayerAnchor.Player.Stamina;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts
{
    
    [CreateAssetMenu(fileName = "StaminaPowerBooster_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERPOWERBOOSTERS_ASSETS_PATH + "StaminaPowerBooster")]
    public class StaminaPowerBooster : ScriptableObject, IPowerBooster
    {
        [SerializeField, Range(0, 100)] private int _staminaAddAmount = 20; 
        
        private IPlayerStaminaPower _playerStaminaPower;
        
        
        public void Init(IPlayerMediator playerMediator)
        {
            _playerStaminaPower = playerMediator.PlayerStaminaPower;
        }

        public void Apply()
        {
            _playerStaminaPower.AddExtraStamina(_staminaAddAmount);
        }

        public void Remove()
        {
            _playerStaminaPower.RemoveExtraBoosts(_staminaAddAmount);
        }
    }
}