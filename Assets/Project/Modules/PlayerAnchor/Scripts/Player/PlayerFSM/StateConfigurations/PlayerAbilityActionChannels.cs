using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerStateConfigurations
{
    [System.Serializable]
    public class PlayerAbilityActionChannels
    {
        [SerializeField] private EmptyEventChannelAsset _anchorPullChannel;
        [SerializeField] private EmptyEventChannelAsset _dashDroppingAnchorChannel;
        [SerializeField] private EmptyEventChannelAsset _dashDroppingAnchorAttackChannel;
        [SerializeField] private EmptyEventChannelAsset _dashTowardsAnchorChannel;
        [SerializeField] private EmptyEventChannelAsset _specialAttackChannel;
        
        public IEmptyEventChannelDispatcher AnchorPullDispatcher => _anchorPullChannel;
        public IEmptyEventChannelDispatcher DashDroppingAnchorDispatcher => _dashDroppingAnchorChannel;
        public IEmptyEventChannelDispatcher DashDroppingAnchorAttackDispatcher => _dashDroppingAnchorAttackChannel;
        public IEmptyEventChannelDispatcher DashTowardsAnchorDispatcher => _dashTowardsAnchorChannel;
        public IEmptyEventChannelDispatcher SpecialAttackDispatcher => _specialAttackChannel;
    }
}