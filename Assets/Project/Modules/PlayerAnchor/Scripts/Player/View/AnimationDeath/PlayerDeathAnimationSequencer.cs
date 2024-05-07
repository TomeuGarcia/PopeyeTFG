using System;
using System.Collections;
using NaughtyAttributes;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.Camera.CameraZoom;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Player.AnimationDeath
{
    public class PlayerDeathAnimationSequencer : MonoBehaviour
    {
        [Header("TARGET")]
        [SerializeField] private Transform _chainsGoalTarget;

        [Header("CHAINS")] 
        [SerializeField] private Vector3 _positionOffset = new Vector3(0, 2, 0);
        [SerializeField] private Transform _chainsHolder;
        [SerializeField] private PlayerDeathChain[] _playerDeathChains;

        [Header("PARTICLES")] 
        [SerializeField] private Transform _deathEndParticlesHolder;
        [SerializeField, Range(0.01f, 5.0f)] private float _deathEndDelay = 0.2f;
        [SerializeField] private ParticleSystem _deathStartParticles;
        [SerializeField] private ParticleSystem _deathEndParticles;

        [SerializeField] private Material[] _deathMaterials;

        [Header("CAMERA EFFECTS")] 
        [SerializeField] private CameraZoomConfig _chainAppearZoom;
        [SerializeField] private CameraZoomConfig _deathEndZoom;
        [SerializeField] private CameraShakeConfig _deathEndShake;
        private ICameraFunctionalities _cameraFunctionalities;
        
        
        private void Start()
        {
            _cameraFunctionalities = ServiceLocator.Instance.GetService<ICameraFunctionalities>();
            
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                playerDeathChain.Init(_cameraFunctionalities);
            }

            ResetState();
        }

        [Button()]
        private void ResetState()
        {
            _chainsHolder.position = _chainsGoalTarget.position + _positionOffset;
            _deathEndParticlesHolder.position = _chainsGoalTarget.position;
        
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                playerDeathChain.ResetState(_chainsGoalTarget);
            }
            
            _deathStartParticles.Stop();
            _deathEndParticles.Stop();

            foreach (Material deathMaterial in _deathMaterials)
            {
                deathMaterial.SetFloat("_IsEnd", 0.0f);
            }
        }
        
        [Button()]
        private void PlayDeathAnimation()
        {
            ResetState();
            StartCoroutine(DeathAnimation());
        }

        private IEnumerator DeathAnimation()
        {
            _deathStartParticles.Play();
        
            foreach (PlayerDeathChain playerDeathChain in _playerDeathChains)
            {
                yield return new WaitForSeconds(playerDeathChain.Delay);
                StartCoroutine(playerDeathChain.MoveToTarget(_chainsGoalTarget));
                
                _cameraFunctionalities.CameraZoomer.ZoomIn(_chainAppearZoom);
            }
            
            
            
            yield return new WaitForSeconds(_deathEndDelay);
            
            foreach (Material deathMaterial in _deathMaterials)
            {
                deathMaterial.SetFloat("_IsEnd", 1.0f);
            }
            
            _deathStartParticles.Stop();
            _deathEndParticles.Play();
            _cameraFunctionalities.CameraZoomer.ZoomToDefault(_deathEndZoom);
            _cameraFunctionalities.CameraShaker.PlayShake(_deathEndShake);
        }
    }
}