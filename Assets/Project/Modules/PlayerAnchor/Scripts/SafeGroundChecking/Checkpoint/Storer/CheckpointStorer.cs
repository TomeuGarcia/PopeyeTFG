using System;
using Popeye.ProjectHelpers;
using Popeye.Scripts.EventChannels;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    [CreateAssetMenu(fileName = "CheckpointStorer", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "CheckpointStorer")]
    public class CheckpointStorer : ScriptableObject, ICheckpointStorerWrite, ICheckpointStorerRead
    {
        [SerializeField] private EmptyEventChannelAsset _checkpointSetEventChannel;
        
        public ICheckpointData LastSafeCheckpoint { get; private set; }

        public Action OnLastSafeCheckpointChanged;
        
        public void SetLastSafeCheckpoint(ICheckpointData checkpointData)
        {
            LastSafeCheckpoint = checkpointData;
            LastSafeCheckpoint.IncrementTimesUsed();
            OnLastSafeCheckpointChanged?.Invoke();

            if (checkpointData.Notify)
            {
                _checkpointSetEventChannel.RaiseEvent();
            }
        }

    }
}