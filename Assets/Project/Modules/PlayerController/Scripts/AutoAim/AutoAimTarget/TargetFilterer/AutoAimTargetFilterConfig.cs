using System;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerController.AutoAim
{
    [CreateAssetMenu(fileName = "AutoAimTargetFilterConfig", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimTargetFilterConfig")]
    public class AutoAimTargetFilterConfig : ScriptableObject
    {
        [Header("Field of View")]
        [SerializeField, Range(0f, 360f)] private float _acceptanceFieldOfView = 180f;
        
        [Header("Height")]
        [SerializeField, Range(0f, 10f)] private float _acceptanceHeightDistance = 2.5f;
        
        [Header("Line of Sight")]
        [SerializeField, Range(1, 5)] private int _numberOfLineOfSightRays = 1;
        [SerializeField, Range(0f, 2f)] private float _lineOfSighRaysDistanceRange = 0.5f;
        
        public float AcceptanceFieldOfViewDot { get; private set; }
        public float AcceptanceHeightDistance => _acceptanceHeightDistance; 
        public int NumberOfLineOfSightRays => _numberOfLineOfSightRays; 
        public float LineOfSighRaysDistanceRange => _lineOfSighRaysDistanceRange; 


        private void OnValidate()
        {
            AcceptanceFieldOfViewDot = Mathf.Cos(_acceptanceFieldOfView / 2);
        }

        private void Awake()
        {
            OnValidate();
        }
    }
}