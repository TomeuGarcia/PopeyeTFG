using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Modules.WorldElements.DestructiblePlatforms
{
    public class DestructiblePlatformBreaker : MonoBehaviour
    {
        [SerializeField] private DestructiblePlatform.BreakMode _breakMode;
        [SerializeField] private bool _startEnabled = true;
        [SerializeField] private Collider _collider;
        private readonly HashSet<Collider> _queuedCollidersWhileDisabled = new HashSet<Collider>(2);


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

            if (enabled)
            {
                ProcessQueuedColliders();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!enabled)
            {
                _queuedCollidersWhileDisabled.Add(other);
                return;
            }

            TryBreakDestructiblePlatform(other);
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (!enabled && _queuedCollidersWhileDisabled.Contains(other))
            {
                _queuedCollidersWhileDisabled.Remove(other);
            }
        }

        private void ProcessQueuedColliders()
        {
            foreach (Collider other in _queuedCollidersWhileDisabled)
            {
                if (_collider.bounds.Intersects(other.bounds))
                {
                    TryBreakDestructiblePlatform(other);
                }
            }
            _queuedCollidersWhileDisabled.Clear();
        }

        
        private void TryBreakDestructiblePlatform(Collider other)
        {
            if (other.TryGetComponent(out DestructiblePlatform destructiblePlatform))
            {
                destructiblePlatform.StartBreaking(_breakMode);
            }
        }
        
    }
}