using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    [System.Serializable]
    public class AutoAimTargetFilterConfig
    {
        [Header("Field of View")]
        [SerializeField, Range(0f, 360f)] private float _acceptanceFieldOfView = 360f;
        
        [Header("Height")]
        [SerializeField, Range(0f, 10f)] private float _acceptanceHeightDistance = 2.5f;
        
        [Header("Line of Sight")]
        [SerializeField, Range(1, 5)] private int _numberOfLineOfSightRays = 1;
        [SerializeField, Range(0f, 2f)] private float _lineOfSighRaysDistanceRange = 0.5f;
        
        public float AcceptanceFieldOfViewDot { get; private set; }
        public float AcceptanceHeightDistance => _acceptanceHeightDistance; 
        public int NumberOfLineOfSightRays => _numberOfLineOfSightRays; 
        public float LineOfSighRaysDistanceRange => _lineOfSighRaysDistanceRange; 


        public AutoAimTargetFilterConfig()
        {
            OnValidate();
        }
        
        public void OnValidate()
        {
            AcceptanceFieldOfViewDot = Mathf.Cos( (Mathf.Deg2Rad * _acceptanceFieldOfView) / 2);
        }


    }
}