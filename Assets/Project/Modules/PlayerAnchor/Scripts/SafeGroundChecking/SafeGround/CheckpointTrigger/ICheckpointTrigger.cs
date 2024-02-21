using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public interface ICheckpointTrigger
    {
        Vector3 RespawnPosition { get; }
    }
}