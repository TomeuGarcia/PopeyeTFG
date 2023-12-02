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

        private Vector3 LookDirection => transform.up;
        private Vector3 UpDirection => -transform.right;


        public Transform GetSnapTransform()
        {
            return _snapSpot;
        }

        public Vector3 GetSnapPosition()
        {
            return _snapSpot.position;
        }
        
        public Quaternion GetSnapRotation()
        {
            return Quaternion.AngleAxis(-45.0f, LookDirection) * Quaternion.LookRotation(-LookDirection, UpDirection);
        }
        

        public bool CanSnapFromPosition(Vector3 position)
        {
            Vector3 direction = (transform.position - position).normalized;
            float dot = Vector3.Dot(direction, LookDirection);

            return dot > 0.0f;
        }

        public void EnterPrepareForSnapping()
        {
            PlayOpenAnimation(0.2f);
        }

        public void QuitPrepareForSnapping()
        {
            PlayCloseAnimation(0.2f);
        }


        public async UniTaskVoid PlaySnapAnimation(float delay)
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