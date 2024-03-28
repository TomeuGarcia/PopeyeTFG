using Popeye.Scripts.ValueGating;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilitiesUnlocker
    {
        private readonly IGateToggle _playerPullGateToggle;
        private readonly IGateToggle _playerDashGateToggle;
        private readonly IGateToggle _playerSpecialAttackGateToggle;

        public PlayerAbilitiesUnlocker(
            IGateToggle playerPullGateToggle,
            IGateToggle playerDashGateToggle,
            IGateToggle playerSpecialAttackGateToggle
            )
        {
            _playerPullGateToggle = playerPullGateToggle;
            _playerDashGateToggle = playerDashGateToggle;
            _playerSpecialAttackGateToggle = playerSpecialAttackGateToggle;
            
            // Use ScriptableObjects as middle-man event invokers?
            // - Invoke when christal breaks
            // - listen here, and other elements such as tutorial texts, etc.
            // Can make an additional ScriptableObject that Invokes the events, just for testing
            // Can make an additional ScriptableObject to toggle the initial state ("locked/unlocked")
        }

        private void UnlockAnchorPull()
        {
            _playerPullGateToggle.Open();
        }
        
        private void UnlockDash()
        {
            _playerDashGateToggle.Open();
        }
        
        private void UnlockSpecialAttack()
        {
            _playerSpecialAttackGateToggle.Open();
        }
        
        
        
    }
}