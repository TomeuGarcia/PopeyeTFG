using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public interface IPhysicsCastRequirement
    {
        bool HitMeetsRequirement(RaycastHit hit);
    }
}