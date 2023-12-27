using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Popeye.Modules.VFX.ParticleFactories
{
    public class ParticleFactory : IParticleFactory
    {
        private readonly ParticleFactoryConfig _config;

        public ParticleFactory(ParticleFactoryConfig config)
        {
            _config = config;
        }
        
        public void CreateEnemyOnHitParticles()
        {
            
        }

        public void CreateEnemyOnHitSimpleParticle()
        {
            
        }

        public void CreateEnemyOnHitCircleParticle()
        {
            
        }
    }
}