using Popeye.Modules.PlayerAnchor.Chain;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates
{
    public class AnchorStatesBlackboard
    {
        public TransformMotion TransformMotion { get; private set; }
        public AnchorMotionConfig AnchorMotionConfig { get; private set; }
        public AnchorPhysics AnchorPhysics { get; private set; }

        
        public AnchorChain AnchorChain { get; private set; }
        
        public Transform AnchorCarryHolder { get; private set; }
        public Transform AnchorGrabToThrowHolder { get; private set; }

        
        public void Configure(TransformMotion anchorMotion, AnchorMotionConfig anchorMotionConfig,
            AnchorPhysics anchorPhysics, AnchorChain anchorChain,
            Transform anchorCarryHolder, Transform anchorGrabToThrowHolder)
        {
            TransformMotion = anchorMotion;
            AnchorMotionConfig = anchorMotionConfig;
            AnchorPhysics = anchorPhysics;
            AnchorChain = anchorChain;
            AnchorCarryHolder = anchorCarryHolder;
            AnchorGrabToThrowHolder = anchorGrabToThrowHolder;
        }
    }
}