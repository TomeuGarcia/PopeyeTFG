using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatformBreaker : MonoBehaviour
    {
        [SerializeField] private DestructiblePlatform.BreakMode _breakMode;
        [SerializeField] private bool _startEnabled = true;

        private void Awake()
        {
            SetEnabled(_startEnabled);
        }

        public void SetBreakOverTimeMode()
        {
            _breakMode = DestructiblePlatform.BreakMode.BreakOverTime;
        }
        public void SetBreakInstantlyMode()
        {
            _breakMode = DestructiblePlatform.BreakMode.InstantBreak;
        }

        public void SetEnabled(bool isEnabled)
        {
            enabled = isEnabled;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DestructiblePlatform destructiblePlatform))
            {
                destructiblePlatform.StartBreaking(_breakMode);
            }
        }
    }
}