using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Popeye.Core.Services.GameReferences;
using Popeye.Core.Services.ServiceLocator;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.Enemies.VFX
{
    public class EnemyVisuals : MonoBehaviour
    {
        [System.Serializable]
        public class OriginalMeshData
        {
            public MeshRenderer _mesh;
            [HideInInspector] public Material _originalMaterial;
        }

        [SerializeField] private GeneralEnemyVFXConfig _visualConfig;

        [SerializeField] [Tooltip("First mesh on the list will be the one that gets 'hurt'")]
        private List<OriginalMeshData> _originalMeshDatas = new();

        private IParticleFactory _particleFactory;

        private void Awake()
        {
            foreach (var data in _originalMeshDatas)
            {
                data._originalMaterial = data._mesh.material;
            }
        }

        public void Configure(IParticleFactory particleFactory)
        {
            _particleFactory = particleFactory;
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", 1.0f);
        }

        public virtual void PlayHitEffects(float healthCoef01, DamageHit damageHit)
        {
            _originalMeshDatas[0]._mesh.material.SetFloat("_Health", healthCoef01);

            ParticlesHitEffect(damageHit);
            FlashHitEffect().Forget();
        }

        public virtual void PlayDeathEffects(DamageHit damageHit)
        {
            ParticlesHitEffect(damageHit);
        }

        private void ParticlesHitEffect(DamageHit damageHit)
        {
            //TODO: FIX these two things
            //get the    position   of the contact point
            //get the     normal    of the contact poiint
            
            Transform player = ServiceLocator.Instance.GetService<IGameReferences>().GetPlayer();
            Vector3 spawnPos = transform.position + (player.position - transform.position) * 0.3f; //0.3f serves as enemy width
            
            _particleFactory.Create(_visualConfig._splatterParticles, spawnPos, quaternion.identity).LookAt(player);
            _particleFactory.Create(_visualConfig._waveParticles, spawnPos, quaternion.identity).LookAt(player);
        }

        private async UniTaskVoid FlashHitEffect()
        {
            foreach (var flash in _visualConfig._flashSequence)
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
