using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor
{
    public class TransformMotion
    {
        private Transform _moveTransform;

        public Vector3 AnchorPosition => _moveTransform.position;

        
        public void Configure(Transform moveTransform)
        {
            _moveTransform = moveTransform;
        }
        
        
        public void MoveByDisplacement(Vector3 displacement, float duration, Ease ease = Ease.Linear)
        {
            _moveTransform.DOBlendableMoveBy(displacement, duration)
                .SetEase(ease);
        }
        
        public void MoveToPosition(Vector3 position, float duration, Ease ease = Ease.Linear)
        {
            _moveTransform.DOMove(position, duration)
                .SetEase(ease);
        }

        public void MoveAlongPath(Vector3[] path, float duration, Ease ease = Ease.Linear)
        {
            _moveTransform.DOPath(path, duration)
                .SetEase(ease);
        }
        

        public void CancelMovement()
        {
            _moveTransform.DOKill();
        }
        
        public void Unparent()
        {
            _moveTransform.DOKill();
            _moveTransform.SetParent(null);
        }
        public void ParentAndReset(Transform parent, float duration, Ease ease = Ease.Linear)
        {
            _moveTransform.SetParent(parent);

            _moveTransform.DOKill();
            _moveTransform.DOLocalMove(Vector3.zero, duration)
                .SetEase(ease);
            _moveTransform.DOLocalRotateQuaternion(Quaternion.identity, duration)
                .SetEase(ease);
        }
        
    }
}