using System;
using System.Linq;
using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainView : IChainView
    {
        private readonly BoneChainChainViewFSM _stateMachine;
        private readonly BoneChain _boneChain;
        private readonly Vector3[] _updatedPositions;
        
        public BoneChainChainView(BoneChain boneChain, int numberOfBones, float chainDistance, float boneLength,
            Bone bonePrefab, Bone boneEndEffectorPrefab)
        {
            _boneChain = boneChain;
            _boneChain.AwakeConfigure(numberOfBones, true, boneLength, bonePrefab, boneEndEffectorPrefab);
            _boneChain.StartInit();


            _stateMachine = new BoneChainChainViewFSM(boneChain, chainDistance);

            _updatedPositions = new Vector3[numberOfBones];
        }
        
        public void Update(Vector3[] positions)
        {
            _stateMachine.Update(positions, ComputePositionsDistance(positions));

            for (int i = 0; i < _boneChain.NumberOfBones; ++i)
            {
                _updatedPositions[i] = _boneChain.Bones[i].Position;
            }
        }

        public Vector3[] GetUpdatedPositions()
        {
            return _updatedPositions;
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