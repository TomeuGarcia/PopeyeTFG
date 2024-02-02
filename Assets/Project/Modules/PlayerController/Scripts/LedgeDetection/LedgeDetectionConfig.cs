using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController
{
    [CreateAssetMenu(fileName = "LedgeDetectionConfig", 
        menuName = ScriptableObjectsHelper.PLAYERCONTROLLER_ASSETS_PATH + "LedgeDetectionConfig")]
    public class LedgeDetectionConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeProbeForwardDisplacement = 0.6f;
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeGroundProbeDistance = 2.0f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeDistance = 0.3f;
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeStartStopDistance = 0.5f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeFriction = 1.0f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 90.0f)] private float _maxLedgeGroundAngle = 40.0f;

        [Space(15)]
        [Tag] [SerializeField] private string _ignoreLedgeTag;
        
        
        public float LedgeProbeForwardDisplacement => _ledgeProbeForwardDisplacement;
        public float LedgeGroundProbeDistance => _ledgeGroundProbeDistance;
        public float LedgeDistance => _ledgeDistance;
        public float LedgeStartStopDistance => _ledgeStartStopDistance;
        public float LedgeFriction => _ledgeFriction;

        public float MinLedgeDotProduct { get; private set; }

        public string IgnoreLedgeTag => _ignoreLedgeTag;
        

        private void OnValidate()
        {
            MinLedgeDotProduct = Mathf.Cos(_maxLedgeGroundAngle * Mathf.Deg2Rad);
        }

        private void Awake()
        {
            OnValidate();
        }
        
        
        
    }
}