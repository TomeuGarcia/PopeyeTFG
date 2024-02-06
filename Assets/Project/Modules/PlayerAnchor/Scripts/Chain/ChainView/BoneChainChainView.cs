using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainView : IChainView
    {
        private readonly BoneChain _boneChain;
        
        
        public BoneChainChainView(BoneChain boneChain, int numberOfBones)
        {
            _boneChain = boneChain;
            _boneChain.AwakeConfigure(numberOfBones);
            _boneChain.StartInit();
        }
        
        public void Update(Vector3[] positions)
        {
            _boneChain.FromPositions(positions);
        }
    }
}