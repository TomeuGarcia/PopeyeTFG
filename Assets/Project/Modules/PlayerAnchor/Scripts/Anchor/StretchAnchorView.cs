using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class StretchAnchorView : MonoBehaviour, IAnchorView
    {
        [SerializeField] private Transform _meshTransform;

        [SerializeField] private Vector3 _throwScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _pullScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _kickScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _carriedScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _restingOnFloorScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);

        [SerializeField, Range(0f, 5f)] private float _pulledDelay;
        
        public void PlayThrownAnimation(float duration)
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_throwScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_pulledDelay));

            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_pullScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayKickedAnimation(float duration)
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_kickScalePunch, duration, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayCarriedAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_carriedScalePunch, 0.2f, 1)
                .SetEase(Ease.InOutQuad);
        }

        public void PlayRestOnFloorAnimation()
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_restingOnFloorScalePunch, 0.2f, 1)
                .SetEase(Ease.InOutQuad);
        }
    }
}