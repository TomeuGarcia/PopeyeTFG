using AYellowpaper;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.ValueStatSystem.Segmented;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private SegmentedValueStatBar _healthBar;
        [SerializeField] private TimeStepSegmentedValueStatBar _staminaBar;
        
        
        
        
        [SerializeField] private InterfaceReference<IPlayerHealingUI, MonoBehaviour> _playerHealingUI;
        public IPlayerHealingUI PlayerHealingUI => _playerHealingUI.Value;
        
        
        
        public void Configure(AValueStat healthSystem, ATimeStepValueStat staminaStat)
        {
            _healthBar.Init(healthSystem);
            _staminaBar.Init(staminaStat);
        }

    }
}