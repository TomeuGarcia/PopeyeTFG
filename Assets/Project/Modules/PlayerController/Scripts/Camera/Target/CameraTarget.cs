using System;
using UnityEngine;

namespace Popeye.Modules.Camera.Target
{
    public class CameraTarget : MonoBehaviour
    {
        [SerializeField] private CameraTargetConfig _config;

        public Transform TargetTransform => transform;

        private void Awake()
        {
            UpdateTransform();
        }

        private void OnEnable()
        {
            _config.OnValuesChanged += OnValuesChangedEvent;
        }
        private void OnDisable()
        {
            _config.OnValuesChanged -= OnValuesChangedEvent;
        }
        
        private void OnValuesChangedEvent()
        {
            UpdateTransform();
        }
        
        
        private void UpdateTransform()
        {
            TargetTransform.localRotation = _config.LocalRotation;
            
            if (_config.OverrideLocalPosition)
            {
                TargetTransform.localPosition = _config.LocalPosition;                
            }
        }

        
        
    }
}