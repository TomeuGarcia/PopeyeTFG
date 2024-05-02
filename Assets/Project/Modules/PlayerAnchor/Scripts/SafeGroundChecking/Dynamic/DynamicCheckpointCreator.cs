using AYellowpaper;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking.Dynamic
{
    public class DynamicCheckpointCreator : MonoBehaviour, IDynamicCheckpointCreator
    {
        [Header("CHECKPOINT CONFIG")]
        [SerializeField] private InterfaceReference<ICheckpointStorerWrite, ScriptableObject> _checkpointStorer;
        [SerializeField] private CheckpointDataAsset _checkpointData;
        
        [Header("OBJECT TO TRACK")]
        [SerializeField] private Transform _objectToTrack;
    
        public void SetCurrentStateAsCheckpoint()
        {
            _checkpointData.Configure(_objectToTrack.position, true);
            _checkpointStorer.Value.SetLastSafeCheckpoint(_checkpointData);
        }
    }
}