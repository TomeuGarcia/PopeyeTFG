using System;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "PlayerUnlockableAbilitiesConfig", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "PlayerUnlockableAbilitiesConfig")]
    public class PlayerUnlockableAbilitiesConfig : ScriptableObject
    {
        
        public interface IChannelAndState
        {
            EmptyEventChannelAsset Channel { get; }
            bool IsUnlocked { get; }
        }
        
        [System.Serializable]
        public class ChannelAndState : IChannelAndState
        {
            [SerializeField] private EmptyEventChannelAsset _channel;
            [SerializeField] private bool _startsUnlocked;

            public EmptyEventChannelAsset Channel => _channel;
            public bool IsUnlocked { get; private set; }

            public void SetIsUnlocked(bool isUnlocked)
            {
                IsUnlocked = isUnlocked;
            }
            public void SetIsUnlockedToStartValue()
            {
                IsUnlocked = _startsUnlocked;
            }
            private void SetIsUnlockedToTrue()
            {
                IsUnlocked = true;
            }

            public void StartListeningToChannelUpdates()
            {
                _channel.Subscribe(SetIsUnlockedToTrue);
            }
            public void StopListeningToChannelUpdates()
            {
                _channel.Unsubscribe(SetIsUnlockedToTrue);
            }
        }

        private enum LoadMode
        {
            UseGameState,
            UseCurrentState,
            EverythingUnlocked,
        }

        [Header("GENERAL")] 
        [SerializeField] private LoadMode _loadMode = LoadMode.UseGameState;
        
        [Header("CHANNELS & STATES")]
        [SerializeField] private ChannelAndState _anchorPull;
        [SerializeField] private ChannelAndState _dashDroppingAnchor;
        [SerializeField] private ChannelAndState _dashDroppingAnchorAttack;
        [SerializeField] private ChannelAndState _dashTowardsAnchor;
        [SerializeField] private ChannelAndState _anchorSpinAttack;
        [SerializeField] private ChannelAndState _chainSpikesAttack;
        
        
        public IChannelAndState AnchorPull => _anchorPull;
        public IChannelAndState DashTowardsAnchor => _dashTowardsAnchor;
        public IChannelAndState DashDroppingAnchor => _dashDroppingAnchor;
        public IChannelAndState AnchorSpinAttack => _anchorSpinAttack;
        public IChannelAndState ChainSpikesAttack => _chainSpikesAttack;
        public IChannelAndState DashDroppingAnchorAttack => _dashDroppingAnchorAttack;


        public void StartChannelListening()
        {
            _anchorPull.StartListeningToChannelUpdates();
            _dashDroppingAnchor.StartListeningToChannelUpdates();
            _dashDroppingAnchorAttack.StartListeningToChannelUpdates();
            _dashTowardsAnchor.StartListeningToChannelUpdates();
            _anchorSpinAttack.StartListeningToChannelUpdates();
            _chainSpikesAttack.StartListeningToChannelUpdates();
        }
        public void StopChannelListening()
        {
            _anchorPull.StopListeningToChannelUpdates();
            _dashDroppingAnchor.StopListeningToChannelUpdates();
            _dashDroppingAnchorAttack.StopListeningToChannelUpdates();
            _dashTowardsAnchor.StopListeningToChannelUpdates();
            _anchorSpinAttack.StopListeningToChannelUpdates();
            _chainSpikesAttack.StopListeningToChannelUpdates();
        }
        
        
        public void SetupState(bool isTutorial)
        {
            if (_loadMode == LoadMode.UseCurrentState)
            {
                SetStartingStateToAll();
                return;
            }

            if (_loadMode == LoadMode.UseGameState)
            {
                if (isTutorial)
                {
                    SetStateToAll(false);
                }
                else
                {
                    
                }
            }

            if (_loadMode == LoadMode.EverythingUnlocked)
            {
                SetStateToAll(true);
            }
            
        }
        
        private void SetStartingStateToAll()
        {
            SetIsUnlockedToStartValue(_anchorPull);
            SetIsUnlockedToStartValue(_dashDroppingAnchor);
            SetIsUnlockedToStartValue(_dashDroppingAnchorAttack);
            SetIsUnlockedToStartValue(_dashTowardsAnchor);
            SetIsUnlockedToStartValue(_anchorSpinAttack);
            SetIsUnlockedToStartValue(_chainSpikesAttack);
        }
        
        private void SetIsUnlockedToStartValue(ChannelAndState channelAndState)
        {
            channelAndState.SetIsUnlockedToStartValue();
        }
        
        
        private void SetStateToAll(bool isUnlocked)
        {
            SetState(_anchorPull, isUnlocked);
            SetState(_dashDroppingAnchor, isUnlocked);
            SetState(_dashDroppingAnchorAttack, isUnlocked);
            SetState(_dashTowardsAnchor, isUnlocked);
            SetState(_anchorSpinAttack, isUnlocked);
            SetState(_chainSpikesAttack, isUnlocked);
        }

        private void SetState(ChannelAndState channelAndState, bool isUnlocked)
        {
            channelAndState.SetIsUnlocked(isUnlocked);
        }

        
    }
}