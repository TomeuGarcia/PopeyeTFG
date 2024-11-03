using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Scripts.TransformUtilities
{
    public class RigidbodyMotion : IMotionBehaviour
    {
        private Rigidbody _moveRigidbody;
        private Transform _rotateTransform;
        private bool _rigidbodyAlsoRotates;
        
        private Transform Transform => _moveRigidbody.transform;

        public Vector3 Position => _moveRigidbody.position;
        public Vector3 Forward => Transform.forward;
        public Quaternion Rotation => _rigidbodyAlsoRotates ? _moveRigidbody.rotation : _rotateTransform.rotation;

        private bool _performingLocalTween;

        
        public RigidbodyMotion()
        {
            _performingLocalTween = false;
        }

        ~RigidbodyMotion()
        {
            _moveRigidbody.DOKill();
        }

        private Vector3 WorldToLocal(Vector3 position)
        {
            return Transform.InverseTransformPoint(position);
        }
        private Vector3 LocalToWorld(Vector3 position)
        {
            return Transform.TransformPoint(position);
        }
        private Quaternion WorldToLocal(Quaternion rotation)
        {
            return rotation * Quaternion.Inverse(Rotation);
        }
        private Quaternion LocalToWorld(Quaternion rotation)
        {
            return rotation * Rotation;
        }


        public void Configure(Rigidbody moveAndRotateRigidbody)
        {
            _moveRigidbody = moveAndRotateRigidbody;
            _rigidbodyAlsoRotates = true;
        }
        public void Configure(Rigidbody moveRigidbody, Transform rotateTransform)
        {
            _moveRigidbody = moveRigidbody;
            _rotateTransform = rotateTransform;
            _rigidbodyAlsoRotates = false;
        }
        
        
        

        public void SetPositionIgnoringPhysics(Vector3 position)
        {
            _moveRigidbody.transform.position = position;
        }
        public void SetPosition(Vector3 position)
        {
            _moveRigidbody.MovePosition(position);
        }
        public void SetLocalPosition(Vector3 position)
        {
            SetPosition(LocalToWorld(position));
        }
        
        
        public void SetRotationIgnoringPhysics(Quaternion rotation)
        {
            _moveRigidbody.transform.rotation = rotation;
        }
        public void SetRotation(Quaternion rotation)
        {
            if (_rigidbodyAlsoRotates)
            {
                _moveRigidbody.MoveRotation(rotation);
            }
            else
            {
                _rotateTransform.rotation = rotation;
            }
        }
        public void SetLocalRotation(Quaternion rotation)
        {
            SetRotation(LocalToWorld(rotation));
        }
        
        
        public void MoveByDisplacement(Vector3 displacement, float duration, Ease ease = Ease.Linear)
        {
            Vector3 endPosition = Position + displacement;
            _moveRigidbody.DOMove(endPosition, duration)
                .SetEase(ease);
        }
        
        public void MoveToPosition(Vector3 position, float duration, Ease ease = Ease.Linear)
        {
            _moveRigidbody.DOMove(position, duration)
                .SetEase(ease);
        }
        public void MoveToPosition(Vector3 position, float duration, AnimationCurve ease)
        {
            _moveRigidbody.DOMove(position, duration)
                .SetEase(ease);
        }

        public void MoveAlongPath(Vector3[] path, float duration, Ease ease = Ease.Linear)
        {
            CancelMovement();
            _moveRigidbody.DOPath(path, duration)
                .SetEase(ease);
        }
        public void MoveAlongPath(Vector3[] path, float duration, AnimationCurve ease)
        {
            CancelMovement();
            _moveRigidbody.DOPath(path, duration)
                .SetEase(ease);
        }
        public void MoveAndRotateAlongPath(Vector3[] positionPath, Quaternion[] rotationPath, 
            float duration, AnimationCurve ease)
        {
            float rotationStepDuration = duration / rotationPath.Length;
            
            CancelMovement();
            _moveRigidbody.DOPath(positionPath, duration)
                .SetEase(ease)
                .OnWaypointChange((index) =>
                {
                    Rotate(rotationPath[index], rotationStepDuration);
                });
        }

        
        public void Rotate(Quaternion endRotation, float duration, Ease ease = Ease.Linear)
        {
            if (_rigidbodyAlsoRotates)
            {
                _moveRigidbody.DORotate(endRotation.eulerAngles, duration)
                    .SetEase(ease);
            }
            else
            {
                _rotateTransform.DORotateQuaternion(endRotation, duration)
                    .SetEase(ease);
            }
        }

        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            AnimationCurve ease)
        {
            if (_rigidbodyAlsoRotates)
            {
                _moveRigidbody.rotation = startRotation;
                _moveRigidbody.DORotate(endRotation.eulerAngles, duration)
                    .SetEase(ease);
            }
            else
            {
                _rotateTransform.rotation = startRotation;
                _rotateTransform.DORotateQuaternion(endRotation, duration)
                    .SetEase(ease);
            }
        }
        public void RotateStartToEnd(Quaternion startRotation, Quaternion endRotation, float duration,
            Ease ease = Ease.Linear)
        {
            if (_rigidbodyAlsoRotates)
            {
                _moveRigidbody.rotation = startRotation;
                _moveRigidbody.DORotate(endRotation.eulerAngles, duration)
                    .SetEase(ease);
            }
            else
            {
                _rotateTransform.rotation = startRotation;
                _rotateTransform.DORotateQuaternion(endRotation, duration)
                    .SetEase(ease);
            }
        }
        

        public void CancelMovement()
        {
            _moveRigidbody.DOKill();
            if (!_rigidbodyAlsoRotates)
            {
                _rotateTransform.DOKill();
            }
            _performingLocalTween = false;
        }

        public void Parent(Transform parent)
        {
            Transform.SetParent(parent);
        }
        public void Unparent()
        {
            CancelMovement();
            Transform.SetParent(null);
        }
        public void ParentAndReset(Transform parent, float duration, Ease ease = Ease.Linear)
        {
            ParentAndUpdate(parent, Vector3.zero, Quaternion.identity, duration, ease);
        }
        public void ParentAndResetInstantly(Transform parent)
        {
            Parent(parent);
            Transform.localPosition = Vector3.zero;
            Transform.localRotation = Quaternion.identity;
            if (!_rigidbodyAlsoRotates)
            {
                _rotateTransform.rotation = Quaternion.identity;
            }
            _moveRigidbody.Move(Position, Rotation); // Refresh
        }

        public void ParentAndUpdate(Transform parent, Vector3 localPosition, Quaternion localRotation,
            float duration, Ease ease = Ease.Linear)
        {
            Parent(parent);

            CancelMovement();
            LocalMoveAndRotate(localPosition, localRotation, duration, ease).Forget();
        }

        public void SetLocalScale(Vector3 localScale)
        {
            Transform.localScale = localScale;
        }
        public void ResetScale()
        {
            Transform.localScale = Vector3.one;
        }

        private async UniTaskVoid LocalMoveAndRotate(Vector3 endLocalPosition, Quaternion endLocalRotation, 
            float duration, Ease ease)
        {
            _performingLocalTween = true;
            
            Timer moveTimer = new Timer(duration);
            EaseFunction easeFunction = EaseManager.ToEaseFunction(ease);

            Vector3 startLocalPosition = WorldToLocal(Position);
            Quaternion startLocalRotation = WorldToLocal(Rotation);

            
            while (!moveTimer.HasFinished() && _performingLocalTween)
            {
                moveTimer.Update(Time.deltaTime);

                float t = easeFunction(moveTimer.Time, moveTimer.Duration, 
                    DOTween.defaultEaseOvershootOrAmplitude, DOTween.defaultEasePeriod);
                
                Vector3 currentLocalPosition = Vector3.LerpUnclamped(startLocalPosition, endLocalPosition, t);
                SetLocalPosition(currentLocalPosition);

                Quaternion currentLocalRotation = Quaternion.LerpUnclamped(startLocalRotation, endLocalRotation, t);
                SetLocalRotation(currentLocalRotation);
                
                
                await UniTask.Yield();
            }

            if (_performingLocalTween)
            {
                SetLocalPosition(endLocalPosition);
                SetLocalRotation(endLocalRotation);
            
                _performingLocalTween = false; 
            }
        }
    }
}