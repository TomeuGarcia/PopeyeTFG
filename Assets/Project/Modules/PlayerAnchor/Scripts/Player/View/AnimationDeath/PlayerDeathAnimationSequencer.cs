using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using Project.General.Scripts.Core.Services.ScreenFade;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    public class PlayerDeathAnimationSequencer : MonoBehaviour
    {
        [Header("CONFIG")] 
        [Expandable] [SerializeField] private PlayerDeathAnimationSequenceConfig _config;
    
        [Header("TARGET")]
        [SerializeField] private Transform _chainsGoalTarget;

        [Header("CHAINS")] 
        [SerializeField] private Transform _chainsHolder;
        [SerializeField] private PlayerDeathChain[] _playerDeathChains;

        [Header("PARTICLES")] 
        [SerializeField] private Transform _deathEndParticlesHolder;
        [SerializeField] private ParticleSystem _deathStartParticles;
        [SerializeField] private ParticleSystem _deathEndParticles;
        
        private ICameraFunctionalities _cameraFunctionalities;
        
        
        
        public bool IsPlayingDeathAnimation { get; private set; }

        private IScreenFadeService _screenFadeService;
        
        
        private void Start()
        {
            _cameraFunctionalities = ServiceLocator.Instance.GetService<ICameraFunctionalities>();
            _screenFadeService = ServiceLocator.Instance.GetService<IScreenFadeService>();
            
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                playerDeathChain.Init(_cameraFunctionalities);
            }

            _chainsHolder.parent = _chainsGoalTarget;
            _chainsHolder.localPosition = _config.ChainsPositionOffset;
            _deathEndParticlesHolder.parent = _chainsGoalTarget;

            ResetState();
        }

        [Button()]
        private void ResetState()
        {
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                playerDeathChain.ResetState(_chainsGoalTarget);
            }
            
            _deathStartParticles.Stop();
            _deathEndParticles.Stop();

            foreach (Material deathMaterial in _config.DeathMaterials)
            {
                deathMaterial.SetFloat(_config.MaterialsEndPropertyId, 0.0f);
            }
        }
        
        [Button()]
        public void PlayDeathAnimation()
        {
            ResetState();
            StartCoroutine(DeathAnimation());
        }
        private void FinishDeathAnimation()
        {
            ResetState();
        }

        private IEnumerator DeathAnimation()
        {
            IsPlayingDeathAnimation = true;
            
            _deathStartParticles.Play();
        
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                yield return new WaitForSeconds(playerDeathChain.Delay);
                StartCoroutine(playerDeathChain.MoveToTarget(_chainsGoalTarget));
                
                _cameraFunctionalities.CameraZoomer.ZoomIn(_config.ChainAppearZoom);
            }
            
            
            
            yield return new WaitForSeconds(_config.DeathEndDelay);
            
            foreach (Material deathMaterial in _config.DeathMaterials)
            {
                deathMaterial.SetFloat(_config.MaterialsEndPropertyId, 1.0f);
            }
            
            _deathStartParticles.Stop();
            _deathEndParticles.Play();
            _cameraFunctionalities.CameraZoomer.ZoomToDefault(_config.DeathEndZoom);
            _cameraFunctionalities.CameraShaker.PlayShake(_config.DeathEndShake);
            
            IsPlayingDeathAnimation = false;
        }

        public async UniTask FinishAnimation(IPlayerView playerView)
        {
            float playerDisappearDuration =_config.DelayFadeInDuration + _config.FadedOutHalfDuration;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_config.BallGrowDuration));
            playerView.PlayDeathFinishAnimation(playerDisappearDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(_config.DelayFadeInDuration));
            
            _screenFadeService.QueueFadeIn();
            await UniTask.Delay(TimeSpan.FromSeconds(_config.FadedOutHalfDuration));
            
            FinishDeathAnimation();
            await UniTask.Delay(TimeSpan.FromSeconds(_config.FadedOutHalfDuration));
            
            _screenFadeService.QueueFadeOut();
        }
        
    }
}