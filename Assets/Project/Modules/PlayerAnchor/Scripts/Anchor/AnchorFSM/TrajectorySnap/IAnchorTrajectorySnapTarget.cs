using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAnchorTrajectorySnapTarget
    {
        Vector3 GetAimLockPosition();
        Vector3 GetLookDirectionForAimedTargeter();
        Quaternion GetRotationForAimedTargeter();
        Transform GetParentTransformForTargeter();
        bool CanBeAimedFromPosition(Vector3 position);

        Vector3 GetDashEndPosition();
        

        void OnAddedAsAimTarget();
        void OnRemovedFromAimTarget();
        void OnUsedAsAimTarget(float delay);
        void OnStartBeingUsed(Transform user);
        void OnFinishBeingUsed();
        void OnUsedForDash();
    }
}