using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Camera.Target
{
    [CreateAssetMenu(fileName = "CameraTargetConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraTargetConfig")]
    public class CameraTargetConfig : ScriptableObject
    {
        [Header("ROTATION")]
        [SerializeField] private Vector3 _localRotation;

        [Header("POSITION")] 
        [SerializeField] private bool _overrideLocalPosition = true;
        [ShowIf("_overrideLocalPosition")] [SerializeField] private Vector3 _localPosition;
        
        public Quaternion LocalRotation => Quaternion.Euler(_localRotation);
        public bool OverrideLocalPosition => _overrideLocalPosition;
        public Vector3 LocalPosition => _localPosition;
        
        
        public delegate void ConfigEvent(); 
        public ConfigEvent OnValuesChanged;


        private void OnValidate()
        {
            OnValuesChanged?.Invoke();
        }
    }
}