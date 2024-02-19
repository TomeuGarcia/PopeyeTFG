using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public interface IPhysicsCaster
    {
        bool CheckHit(out RaycastHit hit);
        bool CheckHitAtPosition(out RaycastHit hit, Vector3 castOriginPosition);
    }
}