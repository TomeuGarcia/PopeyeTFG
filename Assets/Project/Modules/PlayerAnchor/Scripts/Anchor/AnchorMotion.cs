using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorMotion
    {
        private readonly Transform _anchorMoveTransform;

        public AnchorMotion(Transform anchorMoveTransform)
        {
            _anchorMoveTransform = anchorMoveTransform;
        }
        
        
        public void MoveByDisplacement(Vector3 displacement, float duration, Ease ease = Ease.Linear)
        {
            _anchorMoveTransform.DOBlendableMoveBy(displacement, duration)
                .SetEase(ease);
        }
        
        public void MoveToPosition(Vector3 position, float duration, Ease ease = Ease.Linear)
        {

        }

        public void CancelMovement()
        {
            _anchorMoveTransform.DOKill();
        }

        public void FallDown()
        {
            
        }
    }
}