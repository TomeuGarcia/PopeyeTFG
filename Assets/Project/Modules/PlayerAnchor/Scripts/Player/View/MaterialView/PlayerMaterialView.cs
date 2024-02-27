using System;
using Cysharp.Threading.Tasks;
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
            _config.OnValidate();
            
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
        }

        public void PlayRespawnAnimation()
        {
            //TODO
        }

        public void PlayDeathAnimation()
        {
            //TODO
        }

        public void PlayHealAnimation()
        {
            //TODO
        }

        public void PlayDashAnimation(float duration, Vector3 dashDirection)
        {
            DoPlayDash(duration, dashDirection).Forget();
        }

        private async UniTaskVoid DoPlayDash(float duration, Vector3 dashDirection)
        {
            _rendererTransform.gameObject.SetActive(false);
            await UniTask.Delay(TimeSpan.FromSeconds(duration));
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
            _material.SetFloat(_config.IsTiredPropertyID, isTired ? 1f : 0f);
        }
    }
}