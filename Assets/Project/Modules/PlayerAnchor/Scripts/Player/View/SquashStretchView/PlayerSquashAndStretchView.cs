using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player
{
    public class PlayerSquashAndStretchView : IPlayerView
    {
        private readonly PlayerSquashStretchViewConfig _config;
        private readonly Transform _meshHolderTransform;

        [System.Serializable]
        public class TweenPunchData
        {
            [SerializeField] private Vector3 _punch;
            [SerializeField, Range(0.0f, 5.0f)] private float _duration = 0.3f;
            [SerializeField, Range(1, 10)] private int _vibrato = 5;
            [SerializeField] private Ease _ease = Ease.InOutSine;

            public Vector3 Punch => _punch;
            public float Duration => _duration;
            public int Vibrato => _vibrato;
            public Ease Ease => _ease;
        }
        
        [System.Serializable]
        public class TweenData
        {
            [SerializeField] private Vector3 _value;
            [SerializeField, Range(0.0f, 5.0f)] private float _duration = 0.3f;
            [SerializeField] private Ease _ease = Ease.InOutSine;

            public Vector3 Value => _value;
            public float Duration => _duration;
            public Ease Ease => _ease;
        }



        public PlayerSquashAndStretchView(PlayerSquashStretchViewConfig config, Transform meshHolderTransform)
        {
            _config = config;
            _meshHolderTransform = meshHolderTransform;
        }
        
        
        
        public void StartTired()
        {
        }

        public void EndTired()
        {
        }

        public void PlayTakeDamageAnimation()
        {
            ClearTweens();
            PunchScale(_config.TakeDamageScalePunch);
        }

        public void PlayRespawnAnimation()
        {
            _meshHolderTransform.localRotation = Quaternion.identity;
            _meshHolderTransform.localPosition = Vector3.zero;
        }

        public void PlayDeathAnimation()
        {
            Rotate(_config.DeathRotation);
            LocalMoveBy(_config.DeathMoveBy);
        }

        public void PlayHealAnimation()
        {
            ClearTweens();
            PunchScale(_config.HealScalePunch);
        }

        public void PlayDashAnimation(float duration)
        {
            ClearTweens();
            PunchScale(_config.DashScalePunch, duration);
            PunchRotate(_config.DashRotationPunch, duration);
        }

        public void PlayKickAnimation()
        {
            ClearTweens();
            PunchScale(_config.KickScalePunch);
            PunchRotate(_config.KickRotationPunch);
        }

        public void PlayThrowAnimation()
        {
            ClearTweens();
            PunchScale(_config.ThrowScalePunch);
            PunchRotate(_config.ThrowRotationPunch);
        }

        public async UniTaskVoid PlayPullAnimation(float delay)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            ClearTweens();
            PunchScale(_config.PullScalePunch);
            PunchRotate(_config.PullRotationPunch);
        }

        public void PlayAnchorObstructedAnimation()
        {
            ClearTweens();
            PunchRotate(_config.AnchorObstructedRotationPunch);
        }

        public void PlayEnterIdleAnimation()
        {
        }

        public void PlayExitIdleAnimation()
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


        private void ClearTweens()
        {
            _meshHolderTransform.DOComplete();
        }
        
        private void PunchRotate(TweenPunchData tweenPunchData)
        {
            PunchRotate(tweenPunchData, tweenPunchData.Duration);
        }
        private void PunchScale(TweenPunchData tweenPunchData)
        {
            PunchScale(tweenPunchData, tweenPunchData.Duration);
        }
        
        private void PunchRotate(TweenPunchData tweenPunchData, float duration)
        {
            _meshHolderTransform.DOPunchRotation(tweenPunchData.Punch, duration, tweenPunchData.Vibrato)
                .SetEase(tweenPunchData.Ease);
        }
        private void PunchScale(TweenPunchData tweenPunchData, float duration)
        {
            _meshHolderTransform.DOPunchScale(tweenPunchData.Punch, duration, tweenPunchData.Vibrato)
                .SetEase(tweenPunchData.Ease);
        }

        private void Rotate(TweenData tweenData)
        {
            _meshHolderTransform.DORotate(tweenData.Value, tweenData.Duration)
                .SetEase(tweenData.Ease);
        }
        
        private void LocalMoveBy(TweenData tweenData)
        {
            _meshHolderTransform.DOBlendableLocalMoveBy(tweenData.Value, tweenData.Duration)
                .SetEase(tweenData.Ease);
        }
    }
}