using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAutoAimTarget
    {
        Vector3 GetAimLockPosition();
        Quaternion GetRotationForAimedTargeter();
        Transform GetParentTransformForTargeter();
        bool CanBeAimedFromPosition(Vector3 position);


        void OnAddedAsAimTarget();
        void OnRemovedFromAimTarget();
        void OnUsedAsAimTarget(float delay);
    }
}