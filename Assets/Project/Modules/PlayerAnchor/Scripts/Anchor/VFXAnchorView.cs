using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.Generic.ParticleBehaviours;
using Popeye.Modules.VFX.ParticleFactories;
using Project.Scripts.Time.TimeHitStop;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public class VFXAnchorView : MonoBehaviour, IAnchorView
    {
        [Header("PARTICLE TYPES")]
        [SerializeField] private ParticleTypes _carryTrailParticleType;
        [SerializeField] private ParticleTypes _throwTrailParticleType;
        [SerializeField] private ParticleTypes _throwTrailSoftParticleType;
        [SerializeField] private ParticleTypes _throwHeadParticleType;
        [SerializeField] private ParticleTypes _slamHeadParticleType;
        [SerializeField] private ParticleTypes _slamGroundHitParticleType;
        [SerializeField] private ParticleTypes _slamGroundDecalParticleType;
        [SerializeField] private ParticleTypes _throwGroundDecalParticleType;
        
        [Header("HITSTOP CONFIGS")]
        [SerializeField] private HitStopConfig _hitStopDamageDealt;
        
        [Header("CAMERA SHAKE CONFIGS")]
        [SerializeField] private CameraShakeConfig _shakeConfigDamageDealt;
        
        [Header("REFERENCES")]
        [SerializeField] private Transform _vfxParent;
        [SerializeField] private Transform _specialMotionsTransform;

        [Header("PARAMETERS")]
        [SerializeField] private Vector3 _slamTrailOffset = new Vector3(-0.8f, 0.0f, -0.1f);
        [SerializeField] private Vector3 _throwTrailFallnoffset = new Vector3(-0.8f, 0.0f, -0.1f);
        [SerializeField] private float _throwTrailSpawnDelay = 0.2f;
        [SerializeField] private float _throwTrailFallnDelay = 0.05f;
        [SerializeField] private float _retrieveTrailSpawnDelay = 0.2f;
        private Vector3 _slamTrailFlipOffset;
        
        private IParticleFactory _particleFactory;
        private Transform _unparentedVFXHolder;

        private IHitStopManager _hitStopManager;
        private ICameraShaker _cameraShaker;

        private InterpolatorRecycleParticle _carryTrail;

        public void Configure(IParticleFactory particleFactory, IHitStopManager hitStopManager, ICameraShaker cameraShaker)
        {
            _particleFactory = particleFactory;
            _unparentedVFXHolder = _particleFactory.ParticleParent;
            _hitStopManager = hitStopManager;
            _cameraShaker = cameraShaker;
            
            _slamTrailFlipOffset = new Vector3(-_slamTrailOffset.x, -_slamTrailOffset.y, _slamTrailOffset.z);
            
            PlayCarriedAnimation();
        }
        
        public void ResetView()
        {
            
        }

        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            StopCarry();

            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);
            
            float riseTime = duration / 2.0f;
            float fallTime = duration - riseTime;
            
            await UniTask.Delay(TimeSpan.FromSeconds(riseTime / 2.0f));
            PlayTwistLoopAnimation(0.01f, 2, 0.6f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(riseTime / 2.0f));
            _particleFactory.Create(_slamHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(fallTime));
            Transform groundHit = _particleFactory.Create(_slamGroundHitParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);
            Transform groundDecal = _particleFactory.Create(_slamGroundDecalParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);
            
            RaycastHit raycastHit;
            Physics.Raycast(_vfxParent.position, Vector3.down, out raycastHit, 1.0f);
            groundHit.up = raycastHit.normal;
            groundDecal.up = raycastHit.normal;
            groundDecal.RotateAround(groundDecal.up, UnityEngine.Random.Range(0.0f, 360.0f));
            
            _cameraShaker.PlayShake(_shakeConfigDamageDealt);
        }
        
        
        private async UniTaskVoid PlayTwistLoopAnimation(float delay, int numberOfLoops, float duration)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay));
            
            float durationStep = duration / (numberOfLoops * 2);
            
            _specialMotionsTransform.DOBlendableLocalRotateBy(new Vector3(0, 0, 180), durationStep)
                .SetEase(Ease.Linear)
                .OnComplete(() => 
                    _specialMotionsTransform.DOBlendableLocalRotateBy(new Vector3(0, 0, 0), durationStep)
                        .SetEase(Ease.Linear)
                )
                .SetLoops(numberOfLoops);
        }

        public async UniTaskVoid PlayThrownAnimation(float duration)
        {
            StopCarry();
            
            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);
            await UniTask.Delay(TimeSpan.FromSeconds(_throwTrailSpawnDelay));
            Transform rightTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailFlipOffset, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.0f, duration - _throwTrailSpawnDelay - _throwTrailFallnDelay)));
            Transform fallTrail = _particleFactory.Create(_throwTrailSoftParticleType, _throwTrailFallnoffset, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(_throwTrailFallnDelay));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            fallTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            
            Transform throwDecal = _particleFactory.Create(_throwGroundDecalParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);

            RaycastHit raycastHit;
            Physics.Raycast(_vfxParent.position, Vector3.down, out raycastHit, 1.0f);
            throwDecal.up = raycastHit.normal;
            throwDecal.RotateAround(throwDecal.up, UnityEngine.Random.Range(0.0f, 360.0f));
            
            _cameraShaker.PlayShake(_shakeConfigDamageDealt);
        }
        
        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            StopCarry();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_retrieveTrailSpawnDelay));
            Transform rightTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_throwTrailParticleType, _slamTrailFlipOffset, Quaternion.identity, _vfxParent);
            _particleFactory.Create(_throwHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(duration - _retrieveTrailSpawnDelay, 0)));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
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

        public void OnDamageDealt(DamageHitResult damageHitResult)
        {
            _hitStopManager.QueueHitStop(_hitStopDamageDealt);
        }
    }
}