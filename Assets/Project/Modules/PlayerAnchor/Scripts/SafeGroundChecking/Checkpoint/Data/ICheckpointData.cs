using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    public interface ICheckpointData
    {
        Vector3 Position { get; }

        void IncrementTimesUsed();
    }
}