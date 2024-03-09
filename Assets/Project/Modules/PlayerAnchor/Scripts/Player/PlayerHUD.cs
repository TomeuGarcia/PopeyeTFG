using AYellowpaper;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.ValueStatSystem.Segmented;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private ValueStatBar _healthBar;
        [SerializeField] private TimeValueStatBar _staminaBar;
        
        
        [SerializeField] private SegmentedValueStatBar _newHealthBar;
        [SerializeField] private SegmentedValueStatBar _newHealthBar2;
        
        
        [SerializeField] private InterfaceReference<IPlayerHealingUI, MonoBehaviour> _playerHealingUI;
        public IPlayerHealingUI PlayerHealingUI => _playerHealingUI.Value;
        
        
        
        public void Configure(AValueStat healthSystem, ATimeValueStat staminaStat)
        {
            _healthBar.Init(healthSystem);
            _staminaBar.Init(staminaStat);
            
            _newHealthBar.Init(healthSystem);
            _newHealthBar2.Init(healthSystem);
        }

    }
}