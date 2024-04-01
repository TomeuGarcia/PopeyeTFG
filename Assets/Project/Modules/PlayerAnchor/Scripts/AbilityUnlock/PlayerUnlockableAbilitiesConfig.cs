using System;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;

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
        }

        private enum LoadMode
        {
            UseGameState,
            UseCurrentState
        }

        [Header("GENERAL")] 
        [SerializeField] private LoadMode _loadMode = LoadMode.UseGameState;
        
        [Header("CHANNELS & STATES")]
        [SerializeField] private ChannelAndState _anchorPull;
        [SerializeField] private ChannelAndState _dashDroppingAnchor;
        [SerializeField] private ChannelAndState _dashDroppingAnchorAttack;
        [SerializeField] private ChannelAndState _dashTowardsAnchor;
        [SerializeField] private ChannelAndState _specialAttack;
        
        
        public IChannelAndState AnchorPull => _anchorPull;
        public IChannelAndState DashTowardsAnchor => _dashTowardsAnchor;
        public IChannelAndState DashDroppingAnchor => _dashDroppingAnchor;
        public IChannelAndState SpecialAttack => _specialAttack;
        public IChannelAndState DashDroppingAnchorAttack => _dashDroppingAnchorAttack;
        
        
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
                    SetStateToAll(true);
                }
            }
            
        }
        
        private void SetStartingStateToAll()
        {
            _anchorPull.SetIsUnlockedToStartValue();
            _dashDroppingAnchor.SetIsUnlockedToStartValue();
            _dashDroppingAnchorAttack.SetIsUnlockedToStartValue();
            _dashTowardsAnchor.SetIsUnlockedToStartValue();
            _specialAttack.SetIsUnlockedToStartValue();
        }
        private void SetStateToAll(bool isUnlocked)
        {
            SetState(_anchorPull, isUnlocked);
            SetState(_dashDroppingAnchor, isUnlocked);
            SetState(_dashDroppingAnchorAttack, isUnlocked);
            SetState(_dashTowardsAnchor, isUnlocked);
            SetState(_specialAttack, isUnlocked);
        }

        private void SetState(ChannelAndState channelAndState, bool isUnlocked)
        {
            channelAndState.SetIsUnlocked(isUnlocked);
        }

        
    }
}