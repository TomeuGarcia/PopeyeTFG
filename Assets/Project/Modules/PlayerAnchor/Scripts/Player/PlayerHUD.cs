using AYellowpaper;
using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private ValueStatBar _healthBar;
        [SerializeField] private TimeValueStatBar _staminaBar;
        
        
        [SerializeField] private InterfaceReference<IPlayerHealingUI, MonoBehaviour> _playerHealingUI;
        public IPlayerHealingUI PlayerHealingUI => _playerHealingUI.Value;
        
        public void Configure(AValueStat healthSystem, ATimeValueStat staminaStat)
        {
            _healthBar.Init(healthSystem);
            _staminaBar.Init(staminaStat);
        }
        
        
    }
}