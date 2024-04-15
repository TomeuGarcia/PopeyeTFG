using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;

namespace Popeye.Modules.Enemies.Hazards
{
    public class Explosion : RecyclableObject
    {
        private ICombatManager _combatManager;
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


        public void Configure(ICombatManager combatManager, IParticleFactory particleFactory, ExplosionSize size)
        {
            _combatManager = combatManager;
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