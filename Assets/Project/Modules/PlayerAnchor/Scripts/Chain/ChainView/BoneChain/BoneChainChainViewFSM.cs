using System.Collections.Generic;
using Popeye.InverseKinematics.Bones;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    public class BoneChainChainViewFSM
    {
        private readonly Dictionary<BoneChainChainViewStates, ABoneChainChainViewState> _states;
        private BoneChainChainViewStates _currentState;
        
        
        public BoneChainChainViewFSM(BoneChain boneChain, float chainDistance)
        {
            BoneChainChainViewBlackboard blackboard = new BoneChainChainViewBlackboard(boneChain, chainDistance);
            
            
            _states = new Dictionary<BoneChainChainViewStates, ABoneChainChainViewState>
            {
                { BoneChainChainViewStates.CompletelyHidden, new BoneChainState_CompletelyHidden(blackboard) },
                { BoneChainChainViewStates.CoveringPartialDistance, new BoneChainState_CoveringPartialDistance(blackboard) },
                { BoneChainChainViewStates.CoveringAllDistance, new BoneChainState_CoveringAllDistance(blackboard) }
            };

            _currentState = BoneChainChainViewStates.CompletelyHidden;
            _states[_currentState].Enter();
        }

        public void Update(Vector3[] positions, float positionsDistance)
        {
            ABoneChainChainViewState currentState = _states[_currentState];
            if (currentState.Update(positions, positionsDistance))
            {
                currentState.Exit();
                _currentState = currentState.NextState;
                _states[_currentState].Enter();
            }
        }

    }
}