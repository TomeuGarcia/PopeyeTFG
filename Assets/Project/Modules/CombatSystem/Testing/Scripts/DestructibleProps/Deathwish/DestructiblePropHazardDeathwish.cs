using Popeye.Modules.Enemies.Hazards;
using UnityEngine;

namespace Popeye.Modules.CombatSystem.Testing.Scripts
{
    public class DestructiblePropHazardDeathwish : ADestructiblePropDeathwish
    {
        [Header("HAZARD")]
        [SerializeField] private AHazardSpawner _hazardSpawner;
        
        protected override void DoDeathwish()
        {
            _hazardSpawner.Spawn(transform.position, transform.rotation);
        }
    }
}