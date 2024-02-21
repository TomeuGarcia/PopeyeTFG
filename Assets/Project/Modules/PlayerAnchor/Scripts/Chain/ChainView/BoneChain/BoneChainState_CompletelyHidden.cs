using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainState_CompletelyHidden : ABoneChainChainViewState
    {
        private readonly BoneChainChainViewBlackboard _blackboard;

        public BoneChainState_CompletelyHidden(BoneChainChainViewBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        protected override void DoEnter()
        {
            for (int i = 0; i < _blackboard.BoneChain.NumberOfBones; ++i)
            {
                _blackboard.BoneChain.Bones[i].Hide();
            }
        }

        public override void Exit()
        {

        }

        public override bool Update(Vector3[] positions, float positionsDistance)
        {
            if (positionsDistance > _blackboard.ChainDistanceNotVisible)
            {
                NextState = BoneChainChainViewStates.CoveringPartialDistance;
                return true;
            }
            
            if (positionsDistance > _blackboard.ChainDistanceCompletelyVisible)
            {
                NextState = BoneChainChainViewStates.CoveringAllDistance;
                return true;
            }
            
            return false;
        }

    }
}