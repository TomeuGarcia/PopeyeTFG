using System.Collections.Generic;
using Popeye.Scripts.ValueGating;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlockGroupsManager
    {
        private readonly PlayerAbilityUnlockStateConfig _unlockStateConfig;
        
        private readonly List<PlayerAbilityUnlockGroup> _abilitiesToUnlock;



        public PlayerAbilityUnlockGroupsManager(
            PlayerAbilityUnlockStateConfig unlockStateConfig,
            IGateToggle playerPullGateToggle,
            IGateToggle playerDashTowardsAnchorGateToggle,
            IGateToggle playerDashDroppingAnchorGateToggle,
            IGateToggle playerSpecialAttackGateToggle
            )
        {
            _unlockStateConfig = unlockStateConfig;
            _abilitiesToUnlock = new List<PlayerAbilityUnlockGroup>(4);


            // I don't like this!!! (very prone to error)
            
            // I think I need a bigger manager that initializes this class passing an array of Group that are
            // yet to be unlocked
            
            // That other manager should be the one in charge to instantiate everything, and know about the Gate as a
            // whole, to pass it as IGateToggle or IGateValueReader to the respective consumers
            
            
            if (!_unlockStateConfig.AnchorPull.StartsUnlocked)
            {
                PlayerAbilityUnlockGroup abilityUnlockGroup = new (
                    playerPullGateToggle,
                    _unlockStateConfig.AnchorPull.Channel
                );
                
                _abilitiesToUnlock.Add(abilityUnlockGroup);
            }
            
            if (!_unlockStateConfig.DashTowardsAnchor.StartsUnlocked)
            {
                PlayerAbilityUnlockGroup abilityUnlockGroup = new (
                    playerDashTowardsAnchorGateToggle,
                    _unlockStateConfig.DashTowardsAnchor.Channel
                );
                
                _abilitiesToUnlock.Add(abilityUnlockGroup);
            }
            
            if (!_unlockStateConfig.DashDroppingAnchor.StartsUnlocked)
            {
                PlayerAbilityUnlockGroup abilityUnlockGroup = new (
                    playerDashDroppingAnchorGateToggle,
                    _unlockStateConfig.DashDroppingAnchor.Channel
                );
                
                _abilitiesToUnlock.Add(abilityUnlockGroup);
            }
            
            if (!_unlockStateConfig.SpecialAttack.StartsUnlocked)
            {
                PlayerAbilityUnlockGroup abilityUnlockGroup = new (
                    playerSpecialAttackGateToggle,
                    _unlockStateConfig.SpecialAttack.Channel
                );
                
                _abilitiesToUnlock.Add(abilityUnlockGroup);
            }

            
            

            
            // Use ScriptableObjects as middle-man event invokers?
            // - Invoke when christal breaks
            // - listen here, and other elements such as tutorial texts, etc.
            // Can make an additional ScriptableObject that Invokes the events, just for testing
            // Can make an additional ScriptableObject to toggle the initial state ("locked/unlocked")
        }

        
    }
}