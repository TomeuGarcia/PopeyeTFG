using UnityEngine;

namespace Popeye.Modules.Enemies.Hazards
{
    public interface IHazardSpawner
    {
        void Spawn(Vector3 position, Quaternion rotation);
    }
}