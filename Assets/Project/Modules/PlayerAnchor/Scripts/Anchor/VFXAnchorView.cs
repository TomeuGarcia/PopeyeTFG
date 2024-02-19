using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.Anchor.Generic;
using Popeye.Modules.VFX.Anchor.Throw;
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
        [Header("REFERENCES")]
        [SerializeField] private Transform _vfxParent;
        [SerializeField] private AnchorThrowHeadFollower _anchorThrowHeadFollower;
        [SerializeField] private Transform _specialMotionsTransform;

        [Header("CONFIG")]
        [SerializeField] private VFXAnchorViewConfig _vfxAnchorViewConfig;
        
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

            PlayCarriedAnimation();
        }
        
        public void ResetView()
        {
            
        }

        public async UniTaskVoid PlayVerticalHitAnimation(float duration, RaycastHit floorHit)
        {
            StopCarry();

            _particleFactory.Create(_vfxAnchorViewConfig.ThrowHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);
            
            float riseTime = duration / 2.0f;
            float fallTime = duration - riseTime;
            
            await UniTask.Delay(TimeSpan.FromSeconds(riseTime / 2.0f));
            PlayTwistLoopAnimation(0.01f, 2, 0.6f);
            
            await UniTask.Delay(TimeSpan.FromSeconds(riseTime / 2.0f));
            _particleFactory.Create(_vfxAnchorViewConfig.SlamHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(fallTime));
            Transform groundHit = _particleFactory.Create(_vfxAnchorViewConfig.SlamGroundHitParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);
            Transform groundDecal = _particleFactory.Create(_vfxAnchorViewConfig.SlamGroundDecalParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);
            
            RaycastHit raycastHit;
            Physics.Raycast(_vfxParent.position, Vector3.down, out raycastHit, 1.0f);
            groundHit.up = raycastHit.normal;
            groundDecal.up = raycastHit.normal;
            groundDecal.RotateAround(groundDecal.up, UnityEngine.Random.Range(0.0f, 360.0f));
            
            _cameraShaker.PlayShake(_vfxAnchorViewConfig.ShakeConfigDamageDealt);
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
            
            Transform head = _particleFactory.Create(_vfxAnchorViewConfig.ThrowHeadParticleType, Vector3.zero, Quaternion.identity, _vfxParent);
            
            //Used for different way to move the effect pending of more feedback in order to make a decision
            //_anchorThrowHeadFollower.StartFollowing();
            //head.transform.parent = _anchorThrowHeadFollower.transform;
            //head.transform.localPosition = Vector3.zero;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_vfxAnchorViewConfig.ThrowTrailSpawnDelay));
            Transform rightTrail = _particleFactory.Create(_vfxAnchorViewConfig.ThrowTrailParticleType, _vfxAnchorViewConfig.SlamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_vfxAnchorViewConfig.ThrowTrailParticleType, _vfxAnchorViewConfig.SlamTrailFlipOffset, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(0.0f, duration - _vfxAnchorViewConfig.ThrowTrailSpawnDelay - _vfxAnchorViewConfig.ThrowTrailFallnDelay)));
            Transform fallTrail = _particleFactory.Create(_vfxAnchorViewConfig.ThrowTrailSoftParticleType, _vfxAnchorViewConfig.ThrowTrailFallnoffset, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(_vfxAnchorViewConfig.ThrowTrailFallnDelay));
            rightTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            leftTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            fallTrail.gameObject.GetComponent<InterpolatorRecycleParticle>().Play();
            
            Transform throwDecal = _particleFactory.Create(_vfxAnchorViewConfig.ThrowGroundDecalParticleType, _vfxParent.position, Quaternion.identity, _unparentedVFXHolder);

            RaycastHit raycastHit;
            Physics.Raycast(_vfxParent.position, Vector3.down, out raycastHit, 1.0f);
            throwDecal.up = raycastHit.normal;
            throwDecal.RotateAround(throwDecal.up, UnityEngine.Random.Range(0.0f, 360.0f));
            
            //await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            //_anchorThrowHeadFollower.StopFollowing();
        }
        
        public async UniTaskVoid PlayPulledAnimation(float duration)
        {
            StopCarry();
            
            await UniTask.Delay(TimeSpan.FromSeconds(_vfxAnchorViewConfig.RetrieveTrailSpawnDelay));
            Transform rightTrail = _particleFactory.Create(_vfxAnchorViewConfig.RetrieveTrailParticleType, _vfxAnchorViewConfig.SlamTrailOffset, Quaternion.identity, _vfxParent);
            Transform leftTrail = _particleFactory.Create(_vfxAnchorViewConfig.RetrieveTrailParticleType, _vfxAnchorViewConfig.SlamTrailFlipOffset, Quaternion.identity, _vfxParent);

            await UniTask.Delay(TimeSpan.FromSeconds(Mathf.Max(duration - _vfxAnchorViewConfig.RetrieveTrailSpawnDelay, 0)));
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
            _hitStopManager.QueueHitStop(_vfxAnchorViewConfig.HitStopDamageDealt);
        }
    }
}