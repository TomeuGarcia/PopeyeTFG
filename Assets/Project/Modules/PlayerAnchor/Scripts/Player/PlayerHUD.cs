using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;
using Popeye.Modules.ValueStatSystem;
using Popeye.Modules.ValueStatSystem.Segmented;
using Popeye.Modules.ValueStatSystem.StatBarsUI;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerHUD : MonoBehaviour
    {
        [Header("BARS")]
        [SerializeField] private SegmentedValueStatBar _healthBar;
        [SerializeField] private ValueStatSlider _currentHealthSlider; 
        
        [Header("SPECIAL UIs")]
        [SerializeField] private PlayerFocusUI _playerFocusUI;
        public IPlayerFocusUI PlayerFocusUI => _playerFocusUI;
        
        
        
        public void Configure(AValueStat healthSystem, IPlayerFocusState playerFocusState)
        {
            _healthBar.Init(healthSystem);
            _currentHealthSlider.Init(healthSystem);

            _playerFocusUI.Init(playerFocusState);
        }
        
        

    }
}