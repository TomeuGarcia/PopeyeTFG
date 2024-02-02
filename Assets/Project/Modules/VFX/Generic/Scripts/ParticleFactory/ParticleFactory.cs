using System.Collections;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic;
using UnityEngine;

namespace Popeye.Modules.VFX.ParticleFactories
{
    public class ParticleFactory : IParticleFactory
    {
        private readonly ParticleFactoryConfig _config;
        private Dictionary<ParticleTypes, ObjectPool> _typeToPrefab;

        private Transform _particleParent;

        public ParticleFactory(ParticleFactoryConfig config, Transform parent)
        {
            _config = config;
            _particleParent = parent;
            
            _typeToPrefab = _config.GetTypeToPoolDictionary(_particleParent);
        }

        public Transform ParticleParent => _particleParent;

        public Transform Create(ParticleTypes type, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            Transform transform = _typeToPrefab[type].Spawn<RecyclableObject>(position, rotation).transform;
            if (parent != null)
            {
                transform.parent = parent;
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            
            return transform;
        }
    }
}