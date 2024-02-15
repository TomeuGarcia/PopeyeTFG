using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public interface ISafeGroundChecker
    {
        Vector3 LastSafePosition { get; }
        void UpdateChecking(float deltaTime);
    }
}