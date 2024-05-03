using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Timers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public class AnchorHitCheckpointVFXView : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _bellRenderer;
        private Material _bellMaterial;

        [SerializeField] private Light[] _lights;
        
        [SerializeField] private ParticleSystem _transformParticles;
        private Vector3 _transformParticlesStartPosition;
        [SerializeField] private ParticleSystem _hitParticles;

        private AnchorHitCheckpointViewConfig.VFXViewConfig _vfxConfig;

        public void Configure(AnchorHitCheckpointViewConfig.VFXViewConfig vfxConfig,
            bool startAsActiveCheckpoint, bool hasBeenUnlocked)
        {
            _vfxConfig = vfxConfig;
            _bellMaterial = _bellRenderer.material;
            _bellRenderer.material = _bellMaterial;

            foreach (Light light in _lights)
            {
                light.color = _vfxConfig.LockedColor;
            }

            _transformParticles.Stop();
            _transformParticlesStartPosition = _transformParticles.transform.localPosition;
            
            UpdateConvertAnimation(hasBeenUnlocked ? 1 : 0);
        }


        public void PlayFirstTimeUsedAnimation()
        {
            PlayFirstTimeConvertAnimation().Forget();
        }
        
        public void PlayUsedAnimation()
        {
            _hitParticles.Play();
        }

        private async UniTaskVoid PlayFirstTimeConvertAnimation()
        {
            _transformParticles.Play();
        
            Timer convertAnimationTimer = new Timer(_vfxConfig.FirstTimeUsedDuration);

            foreach (Light light in _lights)
            {
                light.DOColor(_vfxConfig.UnlockedColor, _vfxConfig.FirstTimeUsedDuration);
            }

            while (!convertAnimationTimer.HasFinished())
            {
                convertAnimationTimer.Update(Time.deltaTime);
                float t = convertAnimationTimer.GetCounterRatio01();
                
                _transformParticles.transform.localPosition = Vector3.LerpUnclamped(_transformParticlesStartPosition, Vector3.zero, t);
                UpdateConvertAnimation(t);
                

                await UniTask.Yield();
            }
            
            _transformParticles.Stop();
            UpdateConvertAnimation(1);
        }

        private void UpdateConvertAnimation(float t)
        {
            _bellMaterial.SetFloat(_vfxConfig.AnimationTPropertyID, t);
        }
        
    }
}