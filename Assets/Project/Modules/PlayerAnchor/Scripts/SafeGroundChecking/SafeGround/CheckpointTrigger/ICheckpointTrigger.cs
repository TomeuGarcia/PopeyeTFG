using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public interface ICheckpointTrigger
    {
        ICheckpointData CheckpointData { get; }
    }
}