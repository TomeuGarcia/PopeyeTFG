namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PotionsPlayerHealing : IPlayerHealing
    {
        private readonly PlayerHealth _playerHealth;
        private readonly PotionsPlayerHealingConfig _config;
        private readonly IPlayerHealingUI _playerHealingUI;

        private int _currentNumberOfHeals;

        private int MaxNumberOfHeals => _config.NumberOfHeals;
        

        public PotionsPlayerHealing(PlayerHealth playerHealth, PotionsPlayerHealingConfig config,
            IPlayerHealingUI playerHealingUI)
        {
            _playerHealth = playerHealth;
            _config = config;
            _playerHealingUI = playerHealingUI;

            _playerHealingUI.Setup(MaxNumberOfHeals, MaxNumberOfHeals);
            ResetHeals();
        }
        

        public bool CanHeal(out bool hasHealsLeft)
        {
            hasHealsLeft = HasHealsLeft();
            
            return !_playerHealth.IsMaxHealth() && hasHealsLeft;
        }

        public void UseHeal()
        {
            --_currentNumberOfHeals;
            
            _playerHealth.Heal(_config.PotionHealAmount);
            _playerHealingUI.OnHealUsed(_currentNumberOfHeals);

            if (!HasHealsLeft())
            {
                _playerHealingUI.OnHealsExhausted();
            }
        }

        public void ResetHeals()
        {
            _currentNumberOfHeals = MaxNumberOfHeals;
            _playerHealingUI.OnHealsReset(MaxNumberOfHeals);
        }

        private bool HasHealsLeft()
        {
            return _currentNumberOfHeals > 0;
        }
    }
}