using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class FocusPlayerHealing : IPlayerHealing
    {
        private readonly PlayerHealth _playerHealth;
        private readonly FocusPlayerHealingConfig _config;
        private readonly IPlayerFocusSpender _focusSpender;

        
        public FocusPlayerHealing(PlayerHealth playerHealth, FocusPlayerHealingConfig config,
            IPlayerFocusSpender focusSpender)
        {
            _playerHealth = playerHealth;
            _config = config;
            _focusSpender = focusSpender;
        }
        
        public bool CanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = _focusSpender.HasEnoughFocus(_config.RequiredFocusToHeal);
            
            return !_playerHealth.IsMaxHealth() && hasHealsLeft;
        }

        public void UseHeal()
        {
            _playerHealth.Heal(_config.HealAmount);
            _focusSpender.SpendFocus(_config.RequiredFocusToHeal);
        }

        public void ResetHeals()
        {   

        }

    }
}