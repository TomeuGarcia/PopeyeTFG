using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor.AnchorStates.States
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

            float duration = Mathf.Min(0.05f * distance, _blackboard.AnchorMotionConfig.MaxCarriedDuration); 
            
            _blackboard.TransformMotion.ParentAndUpdate(_blackboard.AnchorCarryHolder,
                Vector3.zero, _blackboard.AnchorMotionConfig.CarriedAnchorRotation,
                duration, Ease.InOutSine);
                
            
            _blackboard.AnchorPhysics.DisableCollision();
            
            _blackboard.AnchorChain.SetCarriedView();
        }

        public void Exit()
        {
            
        }
        
    }
}