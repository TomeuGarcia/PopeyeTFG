using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    [CreateAssetMenu(fileName = "PlayerFocusConfig", 
        menuName = ScriptableObjectsHelper.PLAYER_ASSETS_PATH + "PlayerFocusConfig")]
    public class PlayerFocusConfig : ScriptableObject
    {
        [SerializeField, Range(1, 100)] private int _maxFocusAmount = 100;
        [SerializeField, Range(0, 100)] private int _startFocusAmount = 0;
        [SerializeField] private FocusPlayerHealingConfig _healingConfig;
        
        public FocusPlayerHealingConfig HealingConfig => _healingConfig;
        public int MaxFocusAmount => _maxFocusAmount;
        public int StartFocusAmount => _startFocusAmount;

        public int LowestSpendAmount => _healingConfig.RequiredFocusToHeal;
    }
}