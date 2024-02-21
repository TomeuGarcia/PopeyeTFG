using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class ClawAnchorSnapTargetView
    {
        private readonly Transform _clawsTransform;
        private readonly Transform[] _claws;

        public ClawAnchorSnapTargetView(Transform clawsTransform, Transform[] claws)
        {
            _clawsTransform = clawsTransform;
            _claws = claws;
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

        public void PlayOpenAnimation(float duration)
        {
            foreach (var claw in _claws)
            {
                claw.DOComplete();
                Quaternion rotation = claw.localRotation * Quaternion.Euler(0f, 0f, 20f);
                claw.DOLocalRotateQuaternion(rotation, duration)
                    .SetEase(Ease.InOutSine);
            }
        }
        
        public void PlayCloseAnimation(float duration)
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