using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    public interface ICheckpointData
    {
        bool Notify { get; }
        Vector3 Position { get; }

        void IncrementTimesUsed();
    }
}