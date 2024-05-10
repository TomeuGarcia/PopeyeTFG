using System.Collections.Generic;
using Popeye.Scripts.EventChannels;
using Popeye.Scripts.ValueGating;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    public class PlayerAbilityUnlockGroup
    {
        private readonly List<IGateToggle> _abilityGateToggles;
        private readonly IEmptyEventChannelListenEntry _unlockAbilityChannel;
        private bool _isSubscribed;

        public PlayerAbilityUnlockGroup(
            IGateToggle abilityGateToggle, 
            IEmptyEventChannelListenEntry unlockAbilityChannel
            )
        {
            _abilityGateToggles = new List<IGateToggle>(new []{ abilityGateToggle });
            _unlockAbilityChannel = unlockAbilityChannel;
            _isSubscribed = false;
        }

        public void AddGateToggle(IGateToggle abilityGateToggle)
        {
            _abilityGateToggles.Add(abilityGateToggle);
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

        public void DebugUnlockAbilityAndUnsubscribe()
        {
            UnlockAbilityAndUnsubscribe();
        }
        private void UnlockAbilityAndUnsubscribe()        
        {
            UnlockAbility();
            UnsubscribeToEventChannel();
        }
        private void UnlockAbility()
        {
            foreach (IGateToggle abilityGateToggle in _abilityGateToggles)
            {
                abilityGateToggle.Open();
            }
        }
        
    }
}