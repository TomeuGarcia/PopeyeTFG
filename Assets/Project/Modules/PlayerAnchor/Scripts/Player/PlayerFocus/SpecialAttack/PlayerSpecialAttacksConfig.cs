using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spikes;
using Popeye.Modules.PlayerAnchor.Player.PlayerFocus.Spin;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.PlayerFocus
{
    [System.Serializable]
    public class PlayerSpecialAttacksConfig
    {
        [Expandable] [SerializeField] private AnchorSpinAttackConfig _anchorSpinAttackConfig;
        [Expandable] [SerializeField] private ChainSpikesAttackConfig _chainSpikesAttackConfig;
        public AnchorSpinAttackConfig AnchorSpinAttackConfig => _anchorSpinAttackConfig;
        public ChainSpikesAttackConfig ChainSpikesAttackConfig => _chainSpikesAttackConfig;
    }
}