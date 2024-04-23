using AYellowpaper;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.ValueStatSystem.Segmented;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("BARS")]
        [SerializeField] private SegmentedValueStatBar _healthBar;
        [SerializeField] private TimeStepSegmentedValueStatBar _staminaBar;
        
        [Header("SPECIAL UIs")]
        [SerializeField] private PlayerFocusUI _playerFocusUI;
        public IPlayerFocusUI PlayerFocusUI => _playerFocusUI;
        
        
        
        public void Configure(AValueStat healthSystem, ATimeStepValueStat baseStaminaStat, 
            IPlayerFocusState playerFocusState)
        {
            _healthBar.Init(healthSystem);
            _staminaBar.Init(baseStaminaStat);

            _playerFocusUI.Init(playerFocusState);
        }

    }
}