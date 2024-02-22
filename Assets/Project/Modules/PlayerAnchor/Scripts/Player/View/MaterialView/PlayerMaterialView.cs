using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerMaterialView : IPlayerView
    {
        private readonly PlayerMaterialViewConfig _config;
        private readonly Material _material;


        [System.Serializable]
        public class FlickData
        {
            [SerializeField, Range(0.01f, 5.0f)] private float _flickDuration = 0.6f;
            [SerializeField, Range(1, 10)] private int _numberOfFlicks = 3;
            
            public float FlickDuration => _flickDuration;
            public int NumberOfFlicks => _numberOfFlicks;
        }
        


        public PlayerMaterialView(PlayerMaterialViewConfig config, MeshRenderer meshRenderer)
        {
            _config = config;
            _config.OnValidate();
            
            _material = meshRenderer.material;
            
            SetTired(false);
            SetBaseColor(_config.NormalColor);
        }
        
        
        
        public void StartTired()
        {
            SetTired(true);
        }

        public void EndTired()
        {
            SetTired(false);
        }

        public void PlayTakeDamageAnimation()
        {
            FlickBaseColor(_config.TakeDamageFlick, _config.DamagedColor).Forget();
        }

        public void PlayRespawnAnimation()
        {
            
        }

        public void PlayDeathAnimation()
        {
            FlickBaseColor(_config.DeathFlick, _config.DamagedColor, _config.DamagedColor).Forget();
        }

        public void PlayHealAnimation()
        {
            FlickBaseColor(_config.HealFlick, _config.HealColor).Forget();
        }

        public void PlayDashAnimation(float duration)
        {
        }

        public void PlayKickAnimation()
        {
        }

        public void PlayThrowAnimation()
        {
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
        }

        public void PlayAnchorObstructedAnimation()
        {
        }
        
        
        private async UniTask FlickBaseColor(FlickData flickData, Color flickColor)
        {
            await FlickBaseColor(flickData, flickColor, _config.NormalColor);
        }
        private async UniTask FlickBaseColor(FlickData flickData, Color flickColor, Color endColor)
        {
            float halfDuration = flickData.FlickDuration / 2;
            for (int i = 0; i < flickData.NumberOfFlicks; ++i)
            {
                SetBaseColor(flickColor);
                await UniTask.Delay(TimeSpan.FromSeconds(halfDuration));
                SetBaseColor(_config.NormalColor);
                await UniTask.Delay(TimeSpan.FromSeconds(halfDuration));
            }
            
            SetBaseColor(endColor);
        }
        private void SetBaseColor(Color color)
        {
            _material.SetColor(_config.BaseColorPropertyID, color);
        }
        private void SetTired(bool isTired)
        {
            _material.SetFloat(_config.IsTiredPropertyID, isTired ? 1f : 0f);
        }
    }
}