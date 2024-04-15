using Popeye.Core.Pool;
using Popeye.Modules.VFX.ParticleFactories;

namespace Popeye.Modules.Enemies.Hazards
{
    public class Explosion : RecyclableObject
    {
        private IParticleFactory _particleFactory;
        private ExplosionSize _size;
        
        internal override void Init()
        {
            throw new System.NotImplementedException();
        }

        internal override void Release()
        {
            throw new System.NotImplementedException();
        }


        public void Configure(IParticleFactory particleFactory, ExplosionSize size)
        {
            _particleFactory = particleFactory;
            _size = size;
        }

        public void StartExplosion()
        {
            // TODO
            // Parse data from config (damage, scale, etc.) using _size
        }
        
        
    }
}