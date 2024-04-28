using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    [CreateAssetMenu(fileName = "PlayerFocusConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerFocusConfig")]
    public class PlayerFocusConfig : ScriptableObject
    {
        [Header("CONFIGURATION")]
        [SerializeField, Range(1, 100)] private int _maxFocusAmount = 100;
        [SerializeField, Range(0, 100)] private int _startFocusAmount = 0;
        [SerializeField, Range(0, 100)] private int _maxFocusIncreaseAmount = 33;
        
        [SerializeField] private PlayerFocusHealingConfig _healingConfig;
        [SerializeField] private PlayerFocusAttackConfig _attackConfig;
        
        public PlayerFocusHealingConfig HealingConfig => _healingConfig;
        public PlayerFocusAttackConfig AttackConfig => _attackConfig;
        public int MaxFocusAmount => _maxFocusAmount;
        public int StartFocusAmount => _startFocusAmount;
        public int MaxFocusIncreaseAmount => _maxFocusIncreaseAmount;
        public int LowestSpendAmount => Mathf.Min(_healingConfig.RequiredFocusToPerform, _attackConfig.RequiredFocusToPerform);


        [Header("EVENTS")] 
        [SerializeField] private EmptyEventChannelAsset _healthBoostEventChannel;
        [SerializeField] private EmptyEventChannelAsset _focusBoostEventChannel;
        public EmptyEventChannelAsset HealthBoostEventChannel => _healthBoostEventChannel;
        public EmptyEventChannelAsset FocusBoostEventChannel => _focusBoostEventChannel;
    }
}