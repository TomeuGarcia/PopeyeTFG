using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGround
{
    public class CheckpointTriggerChecker : MonoBehaviour, ISafeGroundChecker
    {
        public Vector3 LastSafePosition { get; private set; }


        public void UpdateChecking(float deltaTime)
        {
            
        }
        
        private void Awake()
        {
            SetLastSafePosition(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICheckpointTrigger checkpointTrigger))
            {
                LastSafePosition = checkpointTrigger.RespawnPosition;
            }
        }

        private void SetLastSafePosition(Vector3 position)
        {
            LastSafePosition = position + Vector3.up;
        }

        
    }
}