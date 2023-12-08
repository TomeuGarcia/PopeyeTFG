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
            float distance = Vector3.Distance(_blackboard.TransformMotion.Position,
                _blackboard.AnchorCarryHolder.position);
            
            _blackboard.TransformMotion.ParentAndUpdate(_blackboard.AnchorCarryHolder,
                Vector3.zero, _blackboard.AnchorMotionConfig.CarriedAnchorRotation,
                0.05f * distance, Ease.InOutSine);
            _blackboard.AnchorPhysics.DisableAllPhysics();
            
            _blackboard.AnchorChain.Hide();
        }

        public void Exit()
        {
            
        }
        
    }
}