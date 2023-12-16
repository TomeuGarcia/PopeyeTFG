using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public class StretchAnchorView : MonoBehaviour, IAnchorView
    {
        [SerializeField] private Transform _meshTransform;

        [SerializeField] private Vector3 _verticalHitScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _throwScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _pullScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _kickScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _carriedScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);
        [SerializeField] private Vector3 _restingOnFloorScalePunch = new Vector3(-0.7f, -0.3f, 1.5f);

        [SerializeField, Range(0f, 5f)] private float _pulledDelay;

        [SerializeField] private MeshRenderer _landHitMesh;
        private Material _landHitMaterial;

        private void Awake()
        {
            _landHitMaterial = _landHitMesh.material;
            _landHitMesh.gameObject.SetActive(false);
        }


        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            _meshTransform.DOComplete();
            _meshTransform.DOPunchScale(_verticalHitScalePunch, duration, 1)
                .SetEase(Ease.OutSine);

            duration += 0.2f;
            float delayBeforeHit = duration * 0.7f;
            float delayAfterHit = duration - delayBeforeHit;
            
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeHit));
            
            _landHitMesh.gameObject.SetActive(true);
            _landHitMesh.transform.up = floorHit.normal;
            _landHitMesh.transform.position = floorHit.point + floorHit.normal * 0.01f;
            _landHitMaterial.SetFloat("_StartTime", Time.time);
            _landHitMaterial.SetFloat("_WaveDuration", delayAfterHit*5);
            
            await UniTask.Delay(TimeSpan.FromSeconds(delayAfterHit));
            _landHitMesh.gameObject.SetActive(false);
        }

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