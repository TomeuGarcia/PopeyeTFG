using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.Generic.ParticleBehaviours;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class VFXAnchorView : MonoBehaviour, IAnchorView
    {
        [Header("PARTICLE TYPES")]
        [SerializeField] private ParticleTypes _carryTrailParticleType;
        [SerializeField] private ParticleTypes _throwTrailParticleType;
        [SerializeField] private ParticleTypes _throwHeadParticleType;
        [SerializeField] private ParticleTypes _slamHeadParticleType;
        
        [Header("REFERENCES")]
        [SerializeField] private Transform _vfxParent;
        
        [Header("PARAMETERS")]
        [SerializeField] private Vector3 _slamTrailOffset = new Vector3(-0.8f, 0.0f, -0.1f);
        private Vector3 _slamTrailFlipOffset;
        
        private IParticleFactory _particleFactory => ServiceLocator.Instance.GetService<IParticleFactory>();

        private InterpolatorRecycleParticle _carryTrail;
        
        private void Awake()
        {
            _slamTrailFlipOffset = new Vector3(-_slamTrailOffset.x, -_slamTrailOffset.y, _slamTrailOffset.z);
        }
        
        public void ResetView()
        {
            
        }

        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            //Time.timeScale = 0.2f;
            StopCarry();

            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);
            // Delete this when the able to acces specific times properly?
            float riseTime = duration / 1.5f;
            float fallTime = duration - riseTime;
            
            await UniTask.Delay(TimeSpan.FromSeconds(riseTime));
            Transform rightTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailFlipOffset, Quaternion.identity, _vfxParent);
            _particleFactory.Create(_slamHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(fallTime));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            
            //await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            //Time.timeScale = 1.0f;
        }

        public async UniTaskVoid PlayThrownAnimation(float duration)
        {
            //Time.timeScale = 0.2f;
            StopCarry();
            
            Transform rightTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailFlipOffset, Quaternion.identity, _vfxParent);
            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            
            //await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            //Time.timeScale = 1.0f;
        }

        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            //Time.timeScale = 0.2f;
            StopCarry();
            
            Transform rightTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailFlipOffset, Quaternion.identity, _vfxParent);
            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            
            //await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            //Time.timeScale = 1.0f;
        }

        public void PlayKickedAnimation(float duration)
        {
            
        }

        public void PlayCarriedAnimation()
        {
            //_carryTrail = _particleFactory.Create(_carryTrailParticleType, Vector3.zero, Quaternion.identity, _vfxParent)
                //.gameObject.GetComponent<InterpolatorRecycleParticle>();
        }

        public void StopCarry()
        {
            //_carryTrail.Play();
        }

        public void PlayRestOnFloorAnimation()
        {
            
        }

        public void PlaySpinningAnimation()
        {
            
        }

        public void PlayObstructedAnimation()
        {
            
        }
    }
}