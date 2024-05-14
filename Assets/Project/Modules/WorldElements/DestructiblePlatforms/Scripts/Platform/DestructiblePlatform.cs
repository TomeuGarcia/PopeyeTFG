using System;
using Cysharp.Threading.Tasks;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatform : MonoBehaviour
    {
        public enum BreakMode
        {
            BreakOverTime,
            InstantBreak
        }

        private enum State
        {
            Intact,
            BreakingOverTime,
            Broken
        }
        
        
        [SerializeField] private DestructiblePlatformConfig _config;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Collider[] _groundColliders;

        private DestructiblePlatformCollider _collider;
        private DestructiblePlatformView _view;
        private DestructiblePlatformAudio Audio => _config.Audio;

        private State _currentState;


        private float BreakOverTimeStartDelay => _config.BreakOverTimeStartDelay;
        private float BreakOverTimeDuration => _config.BreakOverTimeDuration;
        private float EnterBrokenStateDelay => _config.EnterBrokenStateDelay;
        private float BrokenStateDuration => _config.BrokenStateDuration;
        
        
        private void Start()
        {
            _meshRenderer.sharedMaterial = _config.AnimationConfig.SharedMaterial;
            
            _collider = new DestructiblePlatformCollider(_groundColliders);
            _view = new DestructiblePlatformView(_meshRenderer.transform, _meshRenderer.material,
                ServiceLocator.Instance.GetService<IParticleFactory>(),
                _config.AnimationConfig);

            _currentState = State.Intact;
        }

        public void StartBreaking(BreakMode breakMode)
        {
            if (_currentState == State.Broken) return;
            
            if (breakMode == BreakMode.BreakOverTime && _currentState != State.BreakingOverTime)
            {
                StartBreakingOverTime().Forget();
            }
            else if (breakMode == BreakMode.InstantBreak)
            {
                StartBreakingInstantly();
            }
        }

        
        private async UniTaskVoid StartBreakingOverTime()
        {
            _currentState = State.BreakingOverTime;            
            Audio.PlayStartBreakingOverTimeSound(gameObject);
            
            await UniTask.Delay(TimeSpan.FromSeconds(BreakOverTimeStartDelay));
            
            _view.StartPlayingBreakingOverTimeAnimation(BreakOverTimeDuration).Forget();

            await UniTask.Delay(TimeSpan.FromSeconds(BreakOverTimeDuration));

            if (_currentState == State.BreakingOverTime)
            {
                _view.FinishPlayingBreakingOverTimeAnimation();
                
                Break().Forget();
            }
        }

        private void StartBreakingInstantly()
        {
            Break().Forget();
        }

        private async UniTaskVoid Break()
        {
            _currentState = State.Broken;
            _view.PlayBreakAnimation();
            Audio.PlayBreakingSound(gameObject);

            await UniTask.Delay(TimeSpan.FromSeconds(EnterBrokenStateDelay));
            
            _collider.DisableCollisions();
            
            StartRegenerating().Forget();
        }



        private async UniTaskVoid StartRegenerating()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(BrokenStateDuration));
            Regenerate();
        }

        private void Regenerate()
        {
            _currentState = State.Intact;
            
            _collider.EnableCollisions();
            _view.PlayRegenerateAnimation();
            Audio.PlayRegeneratingSound(gameObject);
        }
    }
}