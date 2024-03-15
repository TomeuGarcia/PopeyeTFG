using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    public class PlayerPlayerFocusController : IPlayerFocusSpender, IPlayerFocusGainer, IPlayerFocusState
    {
        private readonly PlayerFocusConfig _config;
        private readonly IPlayerFocusUI _focusUI;

        public int MaxFocusAmount => _config.MaxFocusAmount;
        public int CurrentFocusAmount { get; private set; }


        public PlayerPlayerFocusController(PlayerFocusConfig config, IPlayerFocusUI focusUI)
        {
            _config = config;
            _focusUI = focusUI;
        }
        
        
        public bool HasEnoughFocus(int focusAmount)
        {
            return focusAmount >= CurrentFocusAmount;
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