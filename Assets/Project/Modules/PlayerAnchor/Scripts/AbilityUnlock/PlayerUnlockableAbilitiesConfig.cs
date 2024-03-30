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
            [SerializeField] private bool _isUnlocked;

            public EmptyEventChannelAsset Channel => _channel;
            public bool IsUnlocked => _isUnlocked;

            public void SetIsUnlocked(bool isUnlocked)
            {
                _isUnlocked = isUnlocked;
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
        [SerializeField] private ChannelAndState _dashTowardsAnchor;
        [SerializeField] private ChannelAndState _dashDroppingAnchor;
        [SerializeField] private ChannelAndState _specialAttack;
        
        
        public IChannelAndState AnchorPull => _anchorPull;
        public IChannelAndState DashTowardsAnchor => _dashTowardsAnchor;
        public IChannelAndState DashDroppingAnchor => _dashDroppingAnchor;
        public IChannelAndState SpecialAttack => _specialAttack;
        

        private void SetStateToAll(bool isUnlocked)
        {
            _anchorPull.SetIsUnlocked(isUnlocked);
            _dashTowardsAnchor.SetIsUnlocked(isUnlocked);
            _dashDroppingAnchor.SetIsUnlocked(isUnlocked);
            _specialAttack.SetIsUnlocked(isUnlocked);
        }


        public void SetupState(bool isTutorial)
        {
            if (_loadMode == LoadMode.UseCurrentState)
            {
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
        
    }
}