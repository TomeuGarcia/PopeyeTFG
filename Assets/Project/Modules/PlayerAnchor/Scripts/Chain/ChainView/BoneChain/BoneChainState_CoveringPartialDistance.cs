using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainState_CoveringPartialDistance : ABoneChainChainViewState
    {
        private readonly BoneChainChainViewBlackboard _blackboard;
        private BoneChain BoneChain => _blackboard.BoneChain;
        private float ChainDistance => _blackboard.ChainDistance;

        public BoneChainState_CoveringPartialDistance(BoneChainChainViewBlackboard blackboard)
        {
            _blackboard = blackboard;
        }


        protected override void DoEnter()
        {
            
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
            
            if (positionsDistance > _blackboard.ChainDistanceCompletelyVisible)
            {
                NextState = BoneChainChainViewStates.CoveringAllDistance;
                return true;
            }

            UpdateChainPositions(positions, positionsDistance);
            
            return false;
        }

        private void UpdateChainPositions(Vector3[] positions, float positionsDistance)
        {
            float scaler = ChainDistance / positionsDistance;
            int boneCounter = 0;

            int numberOfBonesMinusOne = BoneChain.NumberOfBones - 1;
            float numberOfBonesMinusScaler = Mathf.Min(BoneChain.NumberOfBones - scaler, numberOfBonesMinusOne);
            for (float i = 0; i < numberOfBonesMinusScaler; i += scaler, ++boneCounter)
            {
                int currentIndex = (int)i;
                int currentIndexPlus = currentIndex+1;
                float currentIndexFrac = i % 1f;
                
                int nextIndex = (int)(i+scaler);
                int nextIndexPlus = nextIndex + 1;
                nextIndexPlus = nextIndexPlus > numberOfBonesMinusOne ? numberOfBonesMinusOne : nextIndexPlus;
                
                float nextIndexFrac = (i+scaler) % 1f;
                
                
                Vector3 newCurrentPosition = Vector3.LerpUnclamped(positions[currentIndex], positions[currentIndexPlus], currentIndexFrac);
                Vector3 newNextPosition = Vector3.LerpUnclamped(positions[nextIndex], positions[nextIndexPlus], nextIndexFrac);

                
                Vector3 oldDirection = (BoneChain.Bones[boneCounter + 1].Position - BoneChain.Bones[boneCounter].Position).normalized;
                Vector3 newDirection = (newNextPosition - newCurrentPosition).normalized;

                Vector3 axis = Vector3.Cross(oldDirection, newDirection).normalized;
                float angle = Mathf.Acos(Vector3.Dot(oldDirection, newDirection)) * Mathf.Rad2Deg;

                if (angle > 1.0f)
                {
                    BoneChain.Bones[boneCounter].SetWorldRotation(Quaternion.AngleAxis(angle, axis) * BoneChain.Bones[boneCounter].Rotation);
                }
                
                BoneChain.Bones[boneCounter].Show();
            }

            for (int i = boneCounter-1; i < BoneChain.NumberOfBones; ++i)
            {
                BoneChain.Bones[i].Hide();
            }
        }
    }
}