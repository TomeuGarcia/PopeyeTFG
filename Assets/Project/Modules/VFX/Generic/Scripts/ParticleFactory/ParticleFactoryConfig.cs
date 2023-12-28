using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.VFX.ParticleFactories
{
    [CreateAssetMenu(fileName = "ParticleFactoryConfig", 
        menuName = ScriptableObjectsHelper.VFX_ASSETS_PATH + "ParticleFactoryConfig")]
    
    public class ParticleFactoryConfig : ScriptableObject
    {
        public GameObject simpleParticlePrefab;
        public GameObject circleParticlePrefab;
    }
}