using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardFactory
    {
        AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation);
        ParabolicProjectile CreateParabolicProjectile(Transform origin,Transform targetPosition);
        FlatStraightProjectile CreateFlatStraightProjectile(Vector3 position, Quaternion rotation);
        Explosion CreateExplosion(Vector3 position, Quaternion rotation, ExplosionSize size);
    }
}