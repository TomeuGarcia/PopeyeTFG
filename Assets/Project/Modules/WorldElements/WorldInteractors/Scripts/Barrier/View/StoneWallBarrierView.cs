using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldInteractors
{
    public class StoneWallBarrierView : MonoBehaviour, IBarrierView
    {
        [SerializeField] private StoneBarrierViewConfig _config;
        [SerializeField] private MeshRenderer _barrierRenderer;
        private Material _barrierMaterial;

        public float ActivateDuration => _config.ActivationEase.Duration;


        private void Awake()
        {
            _barrierMaterial = _barrierRenderer.material;
            _barrierMaterial.SetFloat(_config.ActivationAnimationProperty, _config.StartsOn ? 1 : 0);
        }

        public async UniTask PlayActivateAnimation()
        {
            await DoPlayAnimation(0f);
        }

        public async UniTask PlayDeactivateAnimation()
        {
            await DoPlayAnimation(1f);
        }
        
        private async UniTask DoPlayAnimation(float endValue)
        {
            await _barrierMaterial.DOFloat(endValue, _config.ActivationAnimationProperty, 
                    _config.ActivationEase.Duration)
                .SetEase(_config.ActivationEase.Ease)
                .SetUpdate(true)
                .AsyncWaitForCompletion();
        }
        
    }
}