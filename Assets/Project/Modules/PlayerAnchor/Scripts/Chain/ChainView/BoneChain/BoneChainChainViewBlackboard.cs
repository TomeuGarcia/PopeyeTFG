using Popeye.InverseKinematics.Bones;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainViewBlackboard
    {
        public BoneChain BoneChain { get; private set; }
        public float ChainDistance { get; private set; }
        public float ChainDistanceNotVisible { get; private set; }
        public float ChainDistanceCompletelyVisible { get; private set; }



        public BoneChainChainViewBlackboard(BoneChain boneChain, float chainDistance)
        {
            BoneChain = boneChain;
            ChainDistance = chainDistance;

            ChainDistanceNotVisible = 1.0f;
            ChainDistanceCompletelyVisible = chainDistance - 0.1f;
        }
    }
}