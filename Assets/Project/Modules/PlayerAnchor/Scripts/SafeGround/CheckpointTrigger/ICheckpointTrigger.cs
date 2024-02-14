using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    public interface ICheckpointTrigger
    {
        Vector3 RespawnPosition { get; }
    }
}