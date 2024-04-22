using System;
using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.Generic;
using Popeye.Modules.VFX.ParticleFactories;
using Unity.Mathematics;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class Explosion : RecyclableObject
    {
        private ICombatManager _combatManager;
        private IParticleFactory _particleFactory;
        private ExplosionSize _size;
        [SerializeField] private ExplosionHazardConfig _explosionHazardConfig;
        [SerializeField] private float _lifeTime=1;
        [SerializeField] private Collider _collider;
        private DamageHitConfig _damageHitConfig;
        
        internal override void Init()
        {
            _collider.enabled = false;
        }

        internal override void Release()
        {
            _collider.enabled = false;
        }


        public void Configure(ICombatManager combatManager, IParticleFactory particleFactory, ExplosionSize size)
        {
            _combatManager = combatManager;
            _particleFactory = particleFactory;
            _size = size;
        }

        public void StartExplosion()
        {
            float size = _explosionHazardConfig.GetScaleBySize(_size);
            transform.localScale = new Vector3(size, size, size);

            _particleFactory.Create(ParticleTypes.Explosion, transform.position, quaternion.identity);
            Transform decal = _particleFactory.Create(ParticleTypes.ExplosionDecal, transform.position, quaternion.identity);
            
            RaycastHit raycastHit;
            Physics.Raycast(transform.position, Vector3.down, out raycastHit, 1.0f);
            decal.up = raycastHit.normal;
            float randomRotation = UnityEngine.Random.Range(0.0f, 360.0f);
            decal.RotateAround(decal.up, randomRotation);
            
            _collider.enabled = true;
            Invoke("Recycle",_lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            _combatManager.TryDealDamage(other.gameObject, _explosionHazardConfig.GetDamageHitBySize(_size), out DamageHitResult damageHitResult);

        }
    }
}