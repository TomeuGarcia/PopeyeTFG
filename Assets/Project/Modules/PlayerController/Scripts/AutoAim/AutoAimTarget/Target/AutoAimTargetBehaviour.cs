using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetBehaviour : MonoBehaviour, IAutoAimTarget
    {
        [SerializeField] private AutoAimTargetDataConfig _autoAimTargetDataConfig;
        private bool _canBeAimedAt = true;

        public AutoAimTargetDataConfig DataConfig => _autoAimTargetDataConfig;
        public Vector3 Position => transform.position;
        public GameObject GameObject => gameObject;

        
        public bool CanBeAimedAt(Vector3 aimFromPosition)
        {
            return _canBeAimedAt;
        }

        public void SetCanBeAImedAt(bool canBeAimedAt)
        {
            _canBeAimedAt = canBeAimedAt;
        }
        
    }
}