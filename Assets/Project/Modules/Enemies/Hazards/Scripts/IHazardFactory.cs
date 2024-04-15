using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardFactory
    {
        ParabolicProjectile CreateParabolicProjectile(Transform origin,Transform targetPosition,float maxDistance,float minDistance);

        AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation);
    }
}