using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerMaterialView : IPlayerView
    {
        private readonly PlayerMaterialViewConfig _config;
        private readonly Material _material;
        private readonly Transform _rendererTransform;


        [System.Serializable]
        public class FlickData
        {
            [SerializeField, Range(0.01f, 5.0f)] private float _flickDuration = 0.6f;
            [SerializeField, Range(1, 10)] private int _numberOfFlicks = 3;
            
            public float FlickDuration => _flickDuration;
            public int NumberOfFlicks => _numberOfFlicks;
        }
        


        public PlayerMaterialView(PlayerMaterialViewConfig config, Material material, Transform rendererTransform)
        {
            _config = config;
            
            _material = material;
            _rendererTransform = rendererTransform;

            SetTired(false);
            //Might be needed: SetDashing(false);
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
            //TODO
            //Not here, but: muffle sound, vignete...
            
            DoFlick(_config.DamagedPropertyId, 0.1f, 2).Forget();
        }

        private async UniTaskVoid DoFlick(int propertyId, float flickDuration, int numberOfFlicks)
        {
            flickDuration /= 2;
            for (int i = 0; i < numberOfFlicks; ++i)
            {
                _material.SetFloat(propertyId, 1f);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
                _material.SetFloat(propertyId, 0f);
                await UniTask.Delay(TimeSpan.FromSeconds(flickDuration));
            }
        }
        
        

        public void PlayRespawnAnimation()
        {
            //TODO
        }

        public void PlayDeathAnimation()
        {
            //TODO
        }

        public void PlayDeathFinishAnimation(float duration)
        {
            DoPlayDeathFinishAnimation(duration).Forget();
        }
        private async UniTaskVoid DoPlayDeathFinishAnimation(float duration)
        {
            _material.SetFloat(_config.DamagedPropertyId, 1.0f);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            _rendererTransform.gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            
            _material.SetFloat(_config.DamagedPropertyId, 0.0f);
            _rendererTransform.gameObject.SetActive(true);
        }

        public void PlayHealAnimation()
        {
            DoPlayHealAnimation().Forget();
        }

        private async UniTaskVoid DoPlayHealAnimation()
        {
            _material.DOFloat(0.2f, _config.HealProperty, _config.HealAppearTime).SetEase(_config.HealAppearEase);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.HealAppearTime));
            _material.DOFloat(0.0f, _config.HealProperty, _config.HealDisappearTime).SetEase(_config.HealDisappearEase);
        }
        public void PlayStartHealingAnimation(float durationToComplete, int consecutiveHeals)
        {
        }
        public void PlayHealingInterruptedAnimation()
        {
        }

        public void PlaySpecialAttackAnimation()
        {
        }

        public void PlaySpecialAttackFinishAnimation()
        {
        }

        public void PlayStartEnteringSpecialAttackAnimation(float durationToComplete)
        {
        }

        public void PlaySpecialAttackInterruptedAnimation()
        {
        }

        public void PlayDashAnimation(float duration, Vector3 dashDirection)
        {
            DoPlayDash(duration, dashDirection).Forget();
        }

        private async UniTaskVoid DoPlayDash(float duration, Vector3 dashDirection)
        {
            _material.DOFloat(1.0f, _config.DashingProperty, _config.DashMaterialTransitionTime);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DashMaterialTransitionTime));
            _rendererTransform.gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.0f, duration - _config.DashMaterialTransitionTime)));
            _material.DOFloat(0.0f, _config.DashingProperty, _config.DashMaterialTransitionTime);
            _rendererTransform.gameObject.SetActive(true);
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

        public void PlayEnterIdleAnimation()
        {
        }

        public void PlayExitIdleAnimation()
        {
            
        }

        public void UpdateMovingAnimation(float isMovingRatio01)
        {
        }

        public void PlayEnterMovingWithAnchorAnimation()
        {
        }

        public void PlayEnterMovingWithoutAnchorAnimation()
        {
        }

        public void PlayEnterAimingAnimation()
        {
        }

        public void PlayPickUpAnchorAnimation()
        {
        }

        private void SetTired(bool isTired)
        {
            _material.DOFloat(isTired ? 1f : 0f, _config.IsTiredProperty, _config.TiredTransitionTime);
        }
    }
}