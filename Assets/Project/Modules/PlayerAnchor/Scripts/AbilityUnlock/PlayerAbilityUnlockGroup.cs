using Popeye.Scripts.EventChannels;
using Popeye.Scripts.ValueGating;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlockGroup
    {
        private readonly IGateToggle _abilityGateToggle;
        private readonly IEmptyEventChannelListenEntry _unlockAbilityChannel;
        private bool _isSubscribed;

        public PlayerAbilityUnlockGroup(
            IGateToggle abilityGateToggle, 
            IEmptyEventChannelListenEntry unlockAbilityChannel
            )
        {
            _abilityGateToggle = abilityGateToggle;
            _unlockAbilityChannel = unlockAbilityChannel;
            _isSubscribed = false;
            
            
        }

        
        public void StartListeningToUnlock()
        {
            SubscribeToEventChannel();
        }
        
        public void StopListeningToUnlock()
        {
            UnsubscribeToEventChannel();
        }

        private void SubscribeToEventChannel()
        {
            if (_isSubscribed) return;
            _isSubscribed = true;
            
            _unlockAbilityChannel.Subscribe(UnlockAbilityAndUnsubscribe);
        }
        private void UnsubscribeToEventChannel()
        {
            if (!_isSubscribed) return;
            _isSubscribed = false;
            
            _unlockAbilityChannel.Unsubscribe(UnlockAbilityAndUnsubscribe);
        }

        private void UnlockAbilityAndUnsubscribe()
        {
            UnlockAbility();
            UnsubscribeToEventChannel();
        }
        private void UnlockAbility()
        {
            _abilityGateToggle.Open();
        }
        
    }
}