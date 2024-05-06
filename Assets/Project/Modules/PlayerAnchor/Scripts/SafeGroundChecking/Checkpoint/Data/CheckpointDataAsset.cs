using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    [CreateAssetMenu(fileName = "CheckpointData_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "CheckpointDataAsset")]
    public class CheckpointDataAsset : ScriptableObject, ICheckpointData
    {
        [SerializeField] private bool _notify = true;

        public bool Notify => _notify;
        public Vector3 Position { get; private set; }
        public int TimesUsed { get; private set; }

        private void OnEnable()
        {
            ResetTimesUsed();
        }

        public void Configure(Vector3 position, bool resetTimesUsed = false)
        {
            Position = position;

            if (resetTimesUsed)
            {
                ResetTimesUsed();
            }
        }
        
        public void IncrementTimesUsed()
        {
            ++TimesUsed;
        }

        private void ResetTimesUsed()
        {
            TimesUsed = 0;
        }
    }
}