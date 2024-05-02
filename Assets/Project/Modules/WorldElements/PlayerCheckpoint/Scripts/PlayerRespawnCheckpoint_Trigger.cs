using System;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using UnityEngine;


namespace Popeye.Modules.WorldElements
{
    public class PlayerRespawnCheckpoint_Trigger : MonoBehaviour, ICheckpointTrigger
    {
        [SerializeField] private CheckpointDataAsset _checkpointData;
        [SerializeField] private Transform _respawnPoint;
        public ICheckpointData CheckpointData => _checkpointData;


        private void Awake()
        {
            _checkpointData.Configure(_respawnPoint.position);
        }
    }
}


