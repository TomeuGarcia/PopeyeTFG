using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.Camera.CameraShake;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.Enemies.VFX
{
    public class EnemyVisuals : MonoBehaviour
    {
        [System.Serializable]
        public class OriginalMeshData
        {
            public Renderer _mesh;
            [HideInInspector] public Material _originalMaterial;
        }

        [SerializeField] private GeneralEnemyVFXConfig _visualConfig;
        [SerializeField] private Transform _visualCenter;

        [SerializeField] [Tooltip("First mesh on the list will be the one that gets 'hurt'")]
        private List<OriginalMeshData> _originalMeshDatas = new();

        private IParticleFactory _particleFactory;
        private ICameraShaker _cameraShaker;

        private void Awake()
        {
            foreach (var data in _originalMeshDatas)
            {
                data._originalMaterial = data._mesh.material;
            }
        }

        public void Configure(IParticleFactory particleFactory, ICameraShaker cameraShaker)
        {
            _particleFactory = particleFactory;
            _cameraShaker = cameraShaker;
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", 1.0f);
        }

        public virtual void PlayHitEffects(float healthCoef01, DamageHit damageHit)
        {
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", healthCoef01);
            _cameraShaker.PlayShake(_visualConfig.OnHitShakeConfig);

            ParticlesHitEffect(damageHit);
            FlashHitEffect().Forget();
        }

        public virtual void PlayDeathEffects(DamageHit damageHit)
        {
            _cameraShaker.PlayShake(_visualConfig.DeathShakeConfig);
            _particleFactory.Create(_visualConfig.DeathParticles, transform.position, quaternion.identity);
            _particleFactory.Create(_visualConfig.BloodDripDeathParticles, _visualCenter.position, quaternion.identity);
            
            ParticlesHitEffect(damageHit);
        }

        private void ParticlesHitEffect(DamageHit damageHit)
        {
            _particleFactory.Create(_visualConfig.BloodHitSplashParticles, _visualCenter.position, quaternion.identity);
            _particleFactory.Create(_visualConfig.BloodDripHitParticles, _visualCenter.position, quaternion.identity);
        }

        private async UniTaskVoid FlashHitEffect()
        {
            foreach (var flash in _visualConfig.FlashSequence)
            {
                foreach (var data in _originalMeshDatas)
                {
                    data._mesh.material = flash._flashMaterial;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(flash._waitTime));
            }

            foreach (var data in _originalMeshDatas)
            {
                for (int i = 0; i < data._mesh.materials.Length; i++)
                {
                    data._mesh.material = data._originalMaterial;
                }
            }
        }
    }
}
