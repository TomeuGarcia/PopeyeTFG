using AYellowpaper;
using Popeye.Modules.PlayerAnchor.SafeGroundChecking.Checkpoint;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public class CheckpointTriggerChecker : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ICheckpointStorerWrite, ScriptableObject> _checkpointDataStorer;

        public Vector3 LastSafePosition { get; private set; }
        public Vector3 BestSafePosition => LastSafePosition;


        public void UpdateChecking(float deltaTime)
        {
            
        }

        public void UpdateChecking()
        {
            
        }

        private void Awake()
        {
            //SetLastSafePosition(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICheckpointTrigger checkpointTrigger))
            {
                SetLastSafePosition(checkpointTrigger.CheckpointData);
            }
        }

        private void SetLastSafePosition(ICheckpointData checkpointData)
        {
            //LastSafePosition = position + Vector3.up;
            _checkpointDataStorer.Value.SetLastSafeCheckpoint(checkpointData);
        }


        public void SetCurrentStateAsSafeGround()
        {
            //SetLastSafePosition(transform.position);
        }
    }
}