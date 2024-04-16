using Popeye.Modules.ValueStatSystem;
using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class HealthUserHazardDeathwish : AHealthUserDeathwish
    {
        [Header("HAZARD")]
        [SerializeField] private AHazardSpawner _hazardSpawner;
        
        protected override void DoDeathwish()
        {
            _hazardSpawner.Spawn(transform.position, transform.rotation);
        }
    }
}