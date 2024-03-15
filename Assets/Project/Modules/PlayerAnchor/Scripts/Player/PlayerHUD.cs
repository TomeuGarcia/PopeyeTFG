using AYellowpaper;
using Popeye.Modules.PlayerAnchor.Player.PlayerPowerBoosts;
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
        [SerializeField] private TimeStepSegmentedValueStatBar _extraStaminaBar;
        
        [Header("SPECIAL UIs")]
        [SerializeField] private InterfaceReference<IPlayerHealingUI, MonoBehaviour> _playerHealingUI;
        [SerializeField] private InterfaceReference<IPlayerPowerBoosterUI, MonoBehaviour> _playerPowerBoosterUI;
        public IPlayerHealingUI PlayerHealingUI => _playerHealingUI.Value;
        public IPlayerPowerBoosterUI PlayerPowerBoosterUI => _playerPowerBoosterUI.Value;
        
        
        
        public void Configure(AValueStat healthSystem, 
            ATimeStepValueStat baseStaminaStat, ATimeStepValueStat extraStaminaStat)
        {
            _healthBar.Init(healthSystem);
            _staminaBar.Init(baseStaminaStat);
            _extraStaminaBar.Init(extraStaminaStat);
        }

    }
}