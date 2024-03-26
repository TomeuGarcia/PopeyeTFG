using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor
{
    public class TransformMotion
    {
        private Transform _moveTransform;
        private Transform _rotateTransform;

        public Vector3 Position => _moveTransform.position;
        public Vector3 Forward => _moveTransform.forward;
        public Quaternion Rotation => _rotateTransform.rotation;

        ~TransformMotion()
        {
            _moveTransform.DOKill();
            _rotateTransform.DOKill();
        }

        public void Configure(Transform moveAndRotateTransform)
        {
            Configure(moveAndRotateTransform, moveAndRotateTransform);
        }
        public void Configure(Transform moveTransform, Transform rotateTransform)
        {
            _moveTransform = moveTransform;
            _rotateTransform = rotateTransform;
        }

        public void SetPosition(Vector3 position)
        {
            _moveTransform.position = position;
        }
        public void SetLocalPosition(Vector3 position)
        {
            _moveTransform.localPosition = position;
        }
        public void SetRotation(Quaternion rotation)
        {
            _rotateTransform.rotation = rotation;
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
        public void MoveToPosition(Vector3 position, float duration, AnimationCurve ease)
        {
            _moveTransform.DOMove(position, duration)
                .SetEase(ease);
        }

        public void MoveAlongPath(Vector3[] path, float duration, Ease ease = Ease.Linear)
        {
            _moveTransform.DOKill();
            _moveTransform.DOPath(path, duration)
                .SetEase(ease);
        }
        public void MoveAlongPath(Vector3[] path, float duration, AnimationCurve ease)
        {
            _moveTransform.DOKill();
            _moveTransform.DOPath(path, duration)
                .SetEase(ease);
        }
        public void MoveAndRotateAlongPath(Vector3[] positionPath, Quaternion[] rotationPath, 
            float duration, AnimationCurve ease)
        {
            float rotationStepDuration = duration / rotationPath.Length;
            
            _moveTransform.DOKill();
            _moveTransform.DOPath(positionPath, duration)
                .SetEase(ease)
                .OnWaypointChange((index) =>
                {
                    Rotate(rotationPath[index], rotationStepDuration);
                });
        }

        
        public void Rotate(Quaternion endRotation, float duration, Ease ease = Ease.Linear)
        {
            _rotateTransform.DORotateQuaternion(endRotation, duration)
                .SetEase(ease);
        }

        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            AnimationCurve ease)
        {
            _rotateTransform.rotation = startRotation;
            _rotateTransform.DORotateQuaternion(endRotation, duration)
                .SetEase(ease);
        }
        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            Ease ease = Ease.Linear)
        {
            _rotateTransform.rotation = startRotation;
            _rotateTransform.DORotateQuaternion(endRotation, duration)
                .SetEase(ease);
        }
        

        public void CancelMovement()
        {
            _moveTransform.DOKill();
        }

        public void Parent(Transform parent)
        {
            _moveTransform.SetParent(parent);
        }
        public void Unparent()
        {
            _moveTransform.DOKill();
            _moveTransform.SetParent(null);
        }
        public void ParentAndReset(Transform parent, float duration, Ease ease = Ease.Linear)
        {
            ParentAndUpdate(parent, Vector3.zero, Quaternion.identity, duration, ease);
        }

        public void ParentAndUpdate(Transform parent, Vector3 localPosition, Quaternion localRotation,
            float duration, Ease ease = Ease.Linear)
        {
            Parent(parent);

            _moveTransform.DOKill();
            _moveTransform.DOLocalMove(localPosition, duration)
                .SetEase(ease);
            _rotateTransform.DOLocalRotateQuaternion(localRotation, duration)
                .SetEase(ease);
        }

        public void ResetScale()
        {
            _moveTransform.localScale = Vector3.one;
            _rotateTransform.localScale = Vector3.one;
        }
    }
}