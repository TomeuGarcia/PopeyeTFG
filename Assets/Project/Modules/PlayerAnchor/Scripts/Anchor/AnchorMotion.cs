using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class AnchorMotion
    {
        private Transform _anchorMoveTransform;

        public Vector3 AnchorPosition => _anchorMoveTransform.position;

        
        public void Configure(Transform anchorMoveTransform)
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
            _anchorMoveTransform.DOMove(position, duration)
                .SetEase(ease);
        }

        public void MoveAlongPath(Vector3[] path, float duration, Ease ease = Ease.Linear)
        {
            _anchorMoveTransform.DOPath(path, duration)
                .SetEase(ease);
        }
        

        public void CancelMovement()
        {
            _anchorMoveTransform.DOKill();
        }
        
        public void Unparent()
        {
            _anchorMoveTransform.DOKill();
            _anchorMoveTransform.SetParent(null);
        }
        public void ParentAndReset(Transform parent, float duration, Ease ease = Ease.Linear)
        {
            _anchorMoveTransform.SetParent(parent);

            _anchorMoveTransform.DOKill();
            _anchorMoveTransform.DOLocalMove(Vector3.zero, duration)
                .SetEase(ease);
            _anchorMoveTransform.DOLocalRotateQuaternion(Quaternion.identity, duration)
                .SetEase(ease);
        }
        
    }
}