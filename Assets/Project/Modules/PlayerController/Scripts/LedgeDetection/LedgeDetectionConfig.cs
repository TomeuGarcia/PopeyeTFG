using System;
using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using Popeye.Scripts.ObjectTypes;
using UnityEngine;

namespace Popeye.Modules.PlayerController
{
    [CreateAssetMenu(fileName = "LedgeDetectionConfig", 
        menuName = ScriptableObjectsHelper.PLAYERCONTROLLER_ASSETS_PATH + "LedgeDetectionConfig")]
    public class LedgeDetectionConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeProbeForwardDisplacement = 0.6f;
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeProbeBackwardDisplacement = 2.0f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeDistance = 0.3f;
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeStartStopDistance = 0.5f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 10.0f)] private float _ledgeFriction = 1.0f;
        
        [Space(15)]
        [SerializeField, Range(0.0f, 90.0f)] private float _maxLedgeGroundAngle = 40.0f;
        
        [Space(15)]
        [SerializeField] private CollisionProbingConfig _groundProbingConfig;
        [SerializeField] private CollisionProbingConfig _ledgeProbingConfig;
        
        [Space(15)]
        [SerializeField] private ObjectTypeAsset _ignoreLedgeObjectType;
        
        public float LedgeProbeForwardDisplacement => _ledgeProbeForwardDisplacement;
        public float LedgeProbeBackwardDisplacement => _ledgeProbeBackwardDisplacement;
        public float LedgeDistance => _ledgeDistance;
        public float LedgeStartStopDistance => _ledgeStartStopDistance;
        public float LedgeFriction => _ledgeFriction;

        public float MinLedgeDotProduct { get; private set; }
        public CollisionProbingConfig GroundProbingConfig => _groundProbingConfig;
        public CollisionProbingConfig LedgeProbingConfig => _ledgeProbingConfig;
        
        public ObjectTypeAsset IgnoreLedgeObjectType => _ignoreLedgeObjectType;
        

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