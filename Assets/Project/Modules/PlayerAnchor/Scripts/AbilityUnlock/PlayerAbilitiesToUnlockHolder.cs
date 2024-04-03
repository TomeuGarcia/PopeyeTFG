

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilitiesToUnlockHolder
    {
        private readonly PlayerAbilityUnlockGroup[] _abilitiesToUnlock;

        public PlayerAbilitiesToUnlockHolder(PlayerAbilityUnlockGroup[] abilitiesToUnlock)
        {
            _abilitiesToUnlock = abilitiesToUnlock;
        }

        public void StartListeningToUnlock()
        {
            foreach (PlayerAbilityUnlockGroup abilityToUnlockGroup in _abilitiesToUnlock)
            {
                abilityToUnlockGroup.StartListeningToUnlock();
            }
        }
        
        public void StopListeningToUnlock()
        {
            foreach (PlayerAbilityUnlockGroup abilityToUnlockGroup in _abilitiesToUnlock)
            {
                abilityToUnlockGroup.StopListeningToUnlock();
            }
        }


        public void DebugUnlockAll()
        {
            foreach (PlayerAbilityUnlockGroup abilityToUnlockGroup in _abilitiesToUnlock)
            {
                abilityToUnlockGroup.DebugUnlockAbilityAndUnsubscribe();
            }
        }
        
    }
}