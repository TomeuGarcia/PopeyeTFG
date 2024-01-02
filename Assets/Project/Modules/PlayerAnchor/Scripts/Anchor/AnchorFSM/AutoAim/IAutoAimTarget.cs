using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    public interface IAutoAimTarget
    {
        Vector3 GetAimLockPosition();
        Vector3 GetLookDirectionForAimedTargeter();
        Quaternion GetRotationForAimedTargeter();
        Transform GetParentTransformForTargeter();
        bool CanBeAimedFromPosition(Vector3 position);


        void OnAddedAsAimTarget();
        void OnRemovedFromAimTarget();
        void OnUsedAsAimTarget(float delay);
    }
}