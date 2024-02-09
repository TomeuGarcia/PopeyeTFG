using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainState_CoveringAllDistance : ABoneChainChainViewState
    {
        private readonly BoneChainChainViewBlackboard _blackboard;
        private BoneChain BoneChain => _blackboard.BoneChain;

        public BoneChainState_CoveringAllDistance(BoneChainChainViewBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        protected override void DoEnter()
        {
            for (int i = 0; i < _blackboard.BoneChain.NumberOfBones; ++i)
            {
                _blackboard.BoneChain.Bones[i].Show();
            }
        }

        public override void Exit()
        {
            
        }

        public override bool Update(Vector3[] positions, float positionsDistance)
        {
            if (positionsDistance < _blackboard.ChainDistanceNotVisible)
            {
                NextState = BoneChainChainViewStates.CompletelyHidden;
                return true;
            }
            
            if (positionsDistance < _blackboard.ChainDistanceCompletelyVisible)
            {
                NextState = BoneChainChainViewStates.CoveringPartialDistance;
                return true;
            }

            UpdateChainPositions(positions);
            
            return false;
        }


        private void UpdateChainPositions(Vector3[] positions)
        {
            int numberOfBonesMinusOne = BoneChain.NumberOfBones - 1;
            for (int i = 0; i < numberOfBonesMinusOne; i++)
            {
                Vector3 oldDirection = (BoneChain.Bones[i + 1].Position - BoneChain.Bones[i].Position).normalized;
                Vector3 newDirection = (positions[i + 1] - positions[i]).normalized;

                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

                if (angle > 1.0f)
                {
                    BoneChain.Bones[i].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * BoneChain.Bones[i].Rotation);
                }
            }
        }
        
    }
}