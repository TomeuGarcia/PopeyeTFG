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

        public ParticleFactory(ParticleFactoryConfig config, Transform parent)
        {
            _config = config;
            _typeToPrefab = _config.GetTypeToPoolDictionary(parent);
        }

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