using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public interface ISafeGroundChecker
    {
        Vector3 LastSafePosition { get; }
        Vector3 BestSafePosition { get; }
        void UpdateChecking(float deltaTime);
        void UpdateChecking();
    }
}