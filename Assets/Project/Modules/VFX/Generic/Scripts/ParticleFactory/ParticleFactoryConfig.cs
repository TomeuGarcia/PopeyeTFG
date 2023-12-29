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
            [SerializeField] private ParticleTypes _key;
            [SerializeField] private RecyclableObject _value;
            [SerializeField, Min(1)] private int _initialInstances = 10;
            
            public ParticleTypes Key => _key;
            public RecyclableObject Value => _value;
            public int InitialInstances => _initialInstances;
        }
        
        [SerializeField] private ParticleTypeToRecyclable[] _particleTypeToPrefabs;

        public Dictionary<ParticleTypes, ObjectPool> GetTypeToPoolDictionary(Transform parent)
        {
            Dictionary<ParticleTypes, ObjectPool> typeToPool = new(_particleTypeToPrefabs.Length);
            foreach (var particleTypeToPrefab in _particleTypeToPrefabs)
            {
                ObjectPool objectPool = new ObjectPool(particleTypeToPrefab.Value, parent);
                objectPool.Init(particleTypeToPrefab.InitialInstances);
                
                typeToPool.Add(particleTypeToPrefab.Key, objectPool);
                
            }

            return typeToPool;
        }
    }
}