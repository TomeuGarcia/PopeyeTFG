using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.SafeGroundChecking
{
    public class CheckpointTriggerChecker : MonoBehaviour, ISafeGroundChecker
    {
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