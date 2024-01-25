using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    [CreateAssetMenu(fileName = "AutoAimTargetConfig_Data_NAME", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimTargetConfig_Data")]
    public class AutoAimTargetDataConfig : ScriptableObject
    {
        [SerializeField, Range(0f, 90f)] private float _angularSize = 20f;
        [SerializeField, Range(0f, 90f)] private float _angularTargetRegion = 40f;
        [SerializeField, Range(0f, 1f)] public float _centerFlattening = 0.3f;
        
        
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