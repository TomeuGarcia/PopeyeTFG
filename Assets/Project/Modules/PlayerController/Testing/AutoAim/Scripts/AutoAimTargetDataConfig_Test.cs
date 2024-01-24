using System;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [CreateAssetMenu(fileName = "AutoAimTargetDataConfig_Test", 
        menuName = "Popeye/TESTING/AutoAimTargetDataConfig_Test")]
    public class AutoAimTargetDataConfig_Test : ScriptableObject
    {
        [SerializeField, Range(0f, 90f)] private float _angularSize = 20f;
        [SerializeField, Range(0f, 90f)] private float _angularTargetRegion = 20f;
        [SerializeField, Range(0f, 1f)] public float _centerFlattening = 0f;
        
        
        public float HalfAngularSize { get; private set; }
        public float HalfAngularTargetRegion { get; private set; }
        public float HalfFlatCenterAngularTargetRegion { get; private set; }
        

        private void OnValidate()
        {
            HalfAngularSize = _angularSize / 2;
            HalfAngularTargetRegion = _angularTargetRegion / 2;

            HalfFlatCenterAngularTargetRegion = HalfAngularTargetRegion * Mathf.Lerp(0.01f, 0.8f, _centerFlattening);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}