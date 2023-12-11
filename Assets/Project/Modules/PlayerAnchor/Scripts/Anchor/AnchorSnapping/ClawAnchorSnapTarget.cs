using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class ClawAnchorSnapTarget : MonoBehaviour, IAnchorSnapTarget
    {
        [SerializeField] private Transform _snapSpot;
        [SerializeField] private Transform _clawsTransform;
        [SerializeField] private Transform[] _claws;

        private const float ACCEPT_FROM_POSITION_MIN_DOT = 0.1f;
        
        private Vector3 LookDirection => transform.up;
        private Vector3 UpDirection => -transform.right;


        public Transform GetParentTransformForTargeter()
        {
            return _snapSpot;
        }

        public Vector3 GetAimLockPosition()
        {
            return _snapSpot.position;
        }

        public Vector3 GetLookDirectionForAimedTargeter()
        {
            return LookDirection;
        }

        public bool CanBeAimedFromPosition(Vector3 position)
        {
            Vector3 direction = (position - transform.position).normalized;
            float dot = Vector3.Dot(direction, LookDirection);

            return dot > ACCEPT_FROM_POSITION_MIN_DOT;
        }

        public Vector3 GetLookDirection()
        {
            return LookDirection;
        }
        

        public Quaternion GetRotationForAimedTargeter()
        {
            return Quaternion.AngleAxis(45.0f, LookDirection) * Quaternion.LookRotation(-LookDirection, UpDirection);
        }
        

        public bool CanSnapFromPosition(Vector3 position)
        {
            return CanBeAimedFromPosition(position);
        }

        
        public void OnAddedAsAimTarget()
        {
            PlayOpenAnimation(0.2f);
        }

        public void OnRemovedFromAimTarget()
        {
            PlayCloseAnimation(0.2f);
        }

        public void OnUsedAsAimTarget(float delay)
        {
            PlaySnapAnimation(delay).Forget();
        }

        private async UniTaskVoid PlaySnapAnimation(float delay)
        {
            float delay1 = delay * 0.7f;
            float delay2 = delay * 0.2f;

            //PlayOpenAnimation(delay1);
            await UniTask.Delay(MathUtilities.SecondsToMilliseconds(delay1));

            PlayCloseAnimation(delay2);
            await UniTask.Delay(MathUtilities.SecondsToMilliseconds(delay2));

            _clawsTransform.DOPunchScale(new Vector3(0.2f, 0.1f, 0.2f), 0.25f, 5);
        }

        private void PlayOpenAnimation(float duration)
        {
            foreach (var claw in _claws)
            {
                claw.DOComplete();
                Quaternion rotation = claw.localRotation * Quaternion.Euler(0f, 0f, 20f);
                claw.DOLocalRotateQuaternion(rotation, duration)
                    .SetEase(Ease.InOutSine);
            }
        }
        private void PlayCloseAnimation(float duration)
        {
            foreach (var claw in _claws)
            {
                claw.DOComplete();
                Quaternion rotation = claw.localRotation * Quaternion.Euler(0f, 0f, -20f);
                claw.DOLocalRotateQuaternion(rotation, duration)
                    .SetEase(Ease.InOutSine);
            }
        }

        
    }
}