using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.AnchorHitCheckpoint
{
    public interface IAnchorHitCheckpointMediator
    {
        void OnWasHitByAnchor(Vector3 damageSourcePosition);
    }
}