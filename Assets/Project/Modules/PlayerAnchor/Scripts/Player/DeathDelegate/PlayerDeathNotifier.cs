using System.Collections.Generic;

namespace Popeye.Modules.PlayerAnchor.Player.DeathDelegate
{
    public class PlayerDeathNotifier : IPlayerDeathNotifier
    {
        private HashSet<IPlayerDeathDelegate> _subscribedDelegates;

        public PlayerDeathNotifier()
        {
            _subscribedDelegates = new HashSet<IPlayerDeathDelegate>(3);
        }
        
        public void AddDelegate(IPlayerDeathDelegate deathDelegate)
        {
            if (_subscribedDelegates.Contains(deathDelegate))
            {
                return;
            }

            _subscribedDelegates.Add(deathDelegate);
        }

        public void RemoveDelegate(IPlayerDeathDelegate deathDelegate)
        {
            if (!_subscribedDelegates.Contains(deathDelegate))
            {
                return;
            }

            _subscribedDelegates.Remove(deathDelegate);
        }
        
        public void NotifyOnPlayerDied()
        {
            foreach (IPlayerDeathDelegate deathDelegate in _subscribedDelegates)
            {
                deathDelegate.OnPlayerDied();
            }
        }
        public void NotifyOnPlayerRespawnedFromDeath()
        {
            foreach (IPlayerDeathDelegate deathDelegate in _subscribedDelegates)
            {
                deathDelegate.OnPlayerRespawnedFromDeath();
            }
        }
        
        
    }
}