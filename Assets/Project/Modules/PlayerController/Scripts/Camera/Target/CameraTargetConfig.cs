using System;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.Camera.Target
{
    [CreateAssetMenu(fileName = "CameraTargetConfig", 
        menuName = ScriptableObjectsHelper.CAMERA_ASSETS_PATH + "CameraTargetConfig")]
    public class CameraTargetConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _localPosition;
        [SerializeField] private Vector3 _localRotation;
        
        public Vector3 LocalPosition => _localPosition;
        public Quaternion LocalRotation => Quaternion.Euler(_localRotation);
        
        
        public delegate void ConfigEvent(); 
        public ConfigEvent OnValuesChanged;


        private void OnValidate()
        {
            OnValuesChanged?.Invoke();
        }
    }
}