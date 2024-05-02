using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    [CreateAssetMenu(fileName = "CheckpointStorer", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "CheckpointStorer")]
    public class CheckpointStorer : ScriptableObject, ICheckpointStorerWrite, ICheckpointStorerRead
    {
        public ICheckpointData LastSafeCheckpoint { get; private set; }

        public Action OnLastSafeCheckpointChanged;
        
        public void SetLastSafeCheckpoint(ICheckpointData checkpointData)
        {
            LastSafeCheckpoint = checkpointData;
            LastSafeCheckpoint.IncrementTimesUsed();
            OnLastSafeCheckpointChanged?.Invoke();
        }

    }
}