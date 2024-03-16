using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerFocusController : IPlayerFocusController
    {
        private readonly PlayerFocusConfig _config;
        private readonly IPlayerFocusUI _focusUI;

        public int MaxFocusAmount => _config.MaxFocusAmount;
        public int CurrentFocusAmount { get; private set; }


        public PlayerFocusController(PlayerFocusConfig config, IPlayerFocusUI focusUI)
        {
            _config = config;
            _focusUI = focusUI;
            SetCurrentFocusAmount(_config.StartFocusAmount);
        }
        
        
        public bool HasEnoughFocus(int focusAmount)
        {
            return CurrentFocusAmount >= focusAmount;
        }

        public void GainFocus(int focusAmount)
        {
            SetCurrentFocusAmount(CurrentFocusAmount + focusAmount);
            _focusUI.OnFocusGained();
        }

        public void SpendFocus(int focusAmount)
        {
            SetCurrentFocusAmount(CurrentFocusAmount - focusAmount);
            _focusUI.OnFocusSpent();
        }
        
        private void SetCurrentFocusAmount(int focusAmount)
        {
            CurrentFocusAmount = Mathf.Clamp(focusAmount, 0, MaxFocusAmount);
        }

    }
}