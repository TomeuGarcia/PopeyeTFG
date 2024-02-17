using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public interface ISafeGroundPhysicsRequirement
    {
        bool MeetsRequirement(RaycastHit groundHit);
    }
}