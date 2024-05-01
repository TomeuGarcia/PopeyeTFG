using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint
{
    [CreateAssetMenu(fileName = "CheckpointData_NAME", 
        menuName = ScriptableObjectsHelper.PLAYERCHECKPOINTS_ASSETS_PATH + "CheckpointDataAsset")]
    public class CheckpointDataAsset : ScriptableObject, ICheckpointData
    {
        public Vector3 Position { get; private set; }

        public void Configure(Vector3 position)
        {
            Position = position;
        }
    }
}