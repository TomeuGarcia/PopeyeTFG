using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor
{
    public interface IAutoAimTarget
    {
        Vector3 GetAimLockPosition();
    }
}