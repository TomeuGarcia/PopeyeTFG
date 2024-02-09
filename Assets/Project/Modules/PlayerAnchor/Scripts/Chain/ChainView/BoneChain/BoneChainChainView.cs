using System;
using System.Linq;
using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainView : IChainView
    {
        private readonly BoneChainChainViewFSM _stateMachine;
        
        
        public BoneChainChainView(BoneChain boneChain, int numberOfBones, float chainDistance, float boneLength)
        {
            boneChain.AwakeConfigure(numberOfBones, true, boneLength);
            boneChain.StartInit();


            _stateMachine = new BoneChainChainViewFSM(boneChain, chainDistance);
        }
        
        public void Update(Vector3[] positions)
        {
            _stateMachine.Update(positions, ComputePositionsDistance(positions));
        }

        private float ComputePositionsDistance(Vector3[] positions)
        {
            float positionsDistance = 0f;
            for (int i = 1; i < positions.Length; ++i)
            {
                positionsDistance += Vector3.Distance(positions[i - 1], positions[i]);
            }

            return positionsDistance;
        }
    }
}