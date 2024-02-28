using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Pool;
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
        [SerializeField, Range(0.01f, 5.0f)] private float _animationDuration = 0.5f; 

        
        private void Awake()
        {
            _material = _renderer.material;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                PlayAnimation(_animationDuration).Forget();
            }
        }


        public async UniTaskVoid PlayAnimation(float duration)
        {
            PlayGrowAnimation(duration);
            
            float waitDuration = duration * _config.DisappearWait;
            await UniTask.Delay(TimeSpan.FromSeconds(waitDuration));

            float disappearDuration = _config.DisappearDuration;
            PlayDisappearAnimation(disappearDuration);
            
            await UniTask.Delay(TimeSpan.FromSeconds(disappearDuration));
            
            Recycle();
        }

        private void PlayGrowAnimation(float duration)
        {
            _animationTransform.DOScale(Vector3.one * 4, duration)
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
        

        internal override void Init()
        {
            _animationTransform.localScale = Vector3.zero;
            _material.SetFloat(_config.AnimationPropertyId, 0);
        }

        internal override void Release()
        {
            
        }
    }
}