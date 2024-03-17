using Popeye.Modules.PlayerAnchor.Player.PlayerFocus;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class FocusPlayerHealing : IPlayerHealing
    {
        private readonly PlayerHealth _playerHealth;
        private readonly PlayerFocusHealingConfig _config;
        private readonly IPlayerFocusSpender _focusSpender;

        
        public FocusPlayerHealing(PlayerHealth playerHealth, PlayerFocusHealingConfig config,
            IPlayerFocusSpender focusSpender)
        {
            _playerHealth = playerHealth;
            _config = config;
            _focusSpender = focusSpender;
        }
        
        public bool CanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = _focusSpender.HasEnoughFocus(_config.RequiredFocusToPerform);
            
            return !_playerHealth.IsMaxHealth() && hasHealsLeft;
        }

        public void UseHeal()
        {
            _playerHealth.Heal(_config.HealAmount);
            _focusSpender.SpendFocus(_config.RequiredFocusToPerform);
        }

        public void ResetHeals()
        {   

        }

    }
}