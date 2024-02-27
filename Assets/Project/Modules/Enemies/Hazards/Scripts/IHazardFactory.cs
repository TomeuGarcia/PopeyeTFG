using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardFactory
    {
        ParabolicProjectile CreateParabolicProjectile(Transform origin,Transform targetPosition);

        AreaDamageOverTime CreateDamageArea(Vector3 position, Quaternion rotation);
    }
}