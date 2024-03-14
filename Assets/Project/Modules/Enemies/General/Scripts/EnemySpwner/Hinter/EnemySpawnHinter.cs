using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.AudioSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.General
{
    public class EnemySpawnHinter : RecyclableObject
    {
        [Header("REFERENCES")]
        [SerializeField] private Transform _animationTransform;
        [SerializeField] private Renderer _renderer;
        private Material _material;

        [Header("CONFIGURATION")] 
        [SerializeField] private EnemySpawnHinterConfig _config;

        public OneShotFMODSound Sound => _config.Sound;
        
        
        private void Start()
        {
            SetConfiguration(_config);
        }

        public void SetConfiguration(EnemySpawnHinterConfig newConfig)
        {
            _config = newConfig;
            _material = _config.Material;
            _renderer.material = _material;
        }

        public async UniTaskVoid PlayAnimation()
        {
            AnimationSetup();
            PlayGrowAnimation();

            float waitDuration = _config.GrowDuration * _config.DisappearWait;
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));

            float disappearDuration = _config.DisappearDuration;
            PlayDisappearAnimation(disappearDuration);
            
            await UniTask.Delay(TimeSpan.FromSeconds(disappearDuration));
            
            Recycle();
        }

        private void PlayGrowAnimation()
        {
            _animationTransform.DOScale(_config.GrowEndSize, _config.GrowDuration)
                .SetEase(_config.GrowEase);
        }

        private void PlayDisappearAnimation(float duration)
        {
            float t = 0;
            DOTween.To(
                () => t,
                (value) =>
                {
                    t = value;
                    _material.SetFloat(_config.AnimationPropertyId, t);
                },
                1.0f,
                duration
            ).SetEase(_config.DisappearEase);
        }


        private void AnimationSetup()
        {
            _animationTransform.localScale = Vector3.zero;
            _material.SetFloat(_config.AnimationPropertyId, 0);
        }

        internal override void Init()
        {
            
        }

        internal override void Release()
        {
            
        }
    }
}