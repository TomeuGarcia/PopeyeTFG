namespace Popeye.Modules.PlayerAnchor.Player
{
    public class FocusPlayerHealing : IPlayerHealing
    {
        public FocusPlayerHealing(PlayerHealth playerHealth, PlayerHealingConfig config,
            IPlayerHealingUI playerHealingUI)
        {
            
        }
        
        public bool CanHeal(out bool hasHealsLeft)
        {
            throw new System.NotImplementedException();
        }

        public void UseHeal()
        {
            throw new System.NotImplementedException();
        }

        public void ResetHeals()
        {
            throw new System.NotImplementedException();
        }
    }
}