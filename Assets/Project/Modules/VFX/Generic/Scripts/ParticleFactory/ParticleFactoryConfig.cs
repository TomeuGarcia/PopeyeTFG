using System;
using System.Collections.Generic;
using Popeye.Core.Pool;
using Popeye.Modules.VFX.Generic;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.VFX.ParticleFactories
{
    [CreateAssetMenu(fileName = "ParticleFactoryConfig", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "ParticleFactoryConfig")]
    
    public class ParticleFactoryConfig : ScriptableObject
    {
        [System.Serializable]
        class ParticleTypeToRecyclable
        {
            [FormerlySerializedAs("particleType")] [FormerlySerializedAs("_key")] [SerializeField] private ParticleTypes _particleType;
            [FormerlySerializedAs("_value")] [SerializeField] private RecyclableObject _particlePrefab;
            [SerializeField, Min(1)] private int _initialInstances = 10;
            
            public ParticleTypes ParticleType => _particleType;
            public RecyclableObject ParticlePrefab => _particlePrefab;
            public int InitialInstances => _initialInstances;
        }
        
        [SerializeField] private ParticleTypeToRecyclable[] _particleTypeToPrefabs;

        public Dictionary<ParticleTypes, ObjectPool> GetTypeToPoolDictionary(Transform parent)
        {
            Dictionary<ParticleTypes, ObjectPool> typeToPool = new(_particleTypeToPrefabs.Length);
            foreach (var particleTypeToPrefab in _particleTypeToPrefabs)
            {
                ObjectPool objectPool = new ObjectPool(particleTypeToPrefab.ParticlePrefab, parent);
                objectPool.Init(particleTypeToPrefab.InitialInstances);
                
                typeToPool.Add(particleTypeToPrefab.ParticleType, objectPool);
                
            }

            return typeToPool;
        }
    }
}