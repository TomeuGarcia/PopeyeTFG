using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerFocusUI : MonoBehaviour, IPlayerFocusUI
    {
        [SerializeField] private ValueStatBar _tankBar;
        [SerializeField] private PlayerFocusUIEffects _uiEffects;
        
        private IPlayerFocusState _playerFocusState;
        private SimpleValueStat _valueStat;
        
        

        public void Init(IPlayerFocusState playerFocusState)
        {
            _playerFocusState = playerFocusState;
            
            _valueStat = new SimpleValueStat(_playerFocusState.MaxFocusAmount, _playerFocusState.CurrentFocusAmount);
            _tankBar.Init(_valueStat);
        }
        
        
        public void OnFocusGained()
        {
            UpdateValueStat();
        }

        public void OnFocusSpent()
        {
            UpdateValueStat();
        }

        public void OnMaxFocusAmountChanged()
        {
            _valueStat.ResetMaxValue(_playerFocusState.MaxFocusAmount, false);
        }

        public void OnStartHavingEnoughFocusToSpend()
        {
            _uiEffects.SetCanBeUsed();
        }

        public void OnStopHavingEnoughFocusToSpend()
        {
            _uiEffects.SetCanNotBeUsed();
        }


        private void UpdateValueStat()
        {
            _valueStat.SetCurrentValue(_playerFocusState.CurrentFocusAmount);
        }
    }
}