using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    public interface ISafeGroundChecker
    {
        Vector3 LastSafePosition { get; }
        void UpdateChecking(float deltaTime);
    }
}