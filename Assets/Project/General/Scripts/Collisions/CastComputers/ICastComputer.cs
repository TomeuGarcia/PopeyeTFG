using UnityEngine;

namespace Popeye.Scripts.Collisions
{
    public interface ICastComputer
    {
        Vector3 ComputeCastOrigin();
        Vector3 ComputeCastDirection();
    }
}