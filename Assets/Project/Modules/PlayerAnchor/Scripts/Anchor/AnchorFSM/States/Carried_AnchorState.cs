using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorStates.States
{
    public class Carried_AnchorState : IAnchorState
    {
        private readonly AnchorStatesBlackboard _blackboard;

        public Carried_AnchorState(AnchorStatesBlackboard blackboard)
        {
            _blackboard = blackboard;
        }
        
        
        public void Enter()
        {
            _blackboard.AnchorMotion.ParentAndReset(_blackboard.AnchorCarryHolder, 0.2f, Ease.InOutSine);
            _blackboard.AnchorPhysics.DisablePhysics();
            
            _blackboard.AnchorChain.Hide();
        }

        public void Exit()
        {
            
        }
        
    }
}