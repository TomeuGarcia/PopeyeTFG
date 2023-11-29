using Project.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class AnchorStatesBlackboard
    {
        public AnchorMotion AnchorMotion { get; private set; }
        public AnchorPhysics AnchorPhysics { get; private set; }

        
        public AnchorChain AnchorChain { get; private set; }
        
        public Transform AnchorCarryHolder { get; private set; }
        public Transform AnchorGrabToThrowHolder { get; private set; }

        
        public void Configure(AnchorMotion anchorMotion, AnchorPhysics anchorPhysics,
            AnchorChain anchorChain,
            Transform anchorCarryHolder, Transform anchorGrabToThrowHolder)
        {
            AnchorMotion = anchorMotion;
            AnchorPhysics = anchorPhysics;
            AnchorChain = anchorChain;
            AnchorCarryHolder = anchorCarryHolder;
            AnchorGrabToThrowHolder = anchorGrabToThrowHolder;
        }
    }
}