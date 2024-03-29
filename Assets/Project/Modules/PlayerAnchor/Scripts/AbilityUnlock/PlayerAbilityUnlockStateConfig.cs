using System.Collections.Generic;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.AbilityUnlock
{
    [CreateAssetMenu(fileName = "PlayerAbilityUnlockStateConfig", 
        menuName = ScriptableObjectsHelper.PLAYERABILITYUNLOCK_ASSETS_PATH + "PlayerAbilityUnlockStateConfig")]
    public class PlayerAbilityUnlockStateConfig : ScriptableObject
    {
        [System.Serializable]
        public class ChannelAndState
        {
            [SerializeField] private EmptyEventChannelAsset _channel;
            [SerializeField] private bool _startsUnlocked;

            public EmptyEventChannelAsset Channel => _channel;
            public bool StartsUnlocked
            {
                get => _startsUnlocked;
                set => _startsUnlocked = value;
            }
        }
        
        [SerializeField] private ChannelAndState _anchorPull;
        [SerializeField] private ChannelAndState _dashTowardsAnchor;
        [SerializeField] private ChannelAndState _dashDroppingAnchor;
        [SerializeField] private ChannelAndState _specialAttack;
        
        
        public ChannelAndState AnchorPull => _anchorPull;
        public ChannelAndState DashTowardsAnchor => _dashTowardsAnchor;
        public ChannelAndState DashDroppingAnchor => _dashDroppingAnchor;
        public ChannelAndState SpecialAttack => _specialAttack;
        
        
    }
}