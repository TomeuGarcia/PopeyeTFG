using AYellowpaper;
using Popeye.Core.Pool;
using Popeye.Modules.CombatSystem;
using Popeye.Modules.VFX.ParticleFactories;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class FlatStraightProjectile : RecyclableObject
    {
        [SerializeField] private FlatStraightProjectileConfig _config;
        [SerializeField] private DamageTrigger _damageTrigger;
        [SerializeField] private InterfaceReference<IFlatStraightProjectileView, MonoBehaviour> _view;
        private IFlatStraightProjectileView View => _view.Value;
        
        internal override void Init() { }

        internal override void Release() { }

        public void Configure(ICombatManager combatManager, IParticleFactory particleFactory)
        {
            _damageTrigger.Configure(combatManager);
            View.Configure(particleFactory);
        }
        
        
    }
}