using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public class FlatStraightProjectileSpawner : AHazardSpawner
    {
        public override void Spawn(Vector3 position, Quaternion rotation)
        {
            HazardFactory.CreateFlatStraightProjectile(position, rotation);
        }
    }
}