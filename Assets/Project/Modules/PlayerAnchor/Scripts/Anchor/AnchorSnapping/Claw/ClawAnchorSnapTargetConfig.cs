using System;
using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "ClawAnchorSnapTargetConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "ClawAnchorSnapTargetConfig")]
    public class ClawAnchorSnapTargetConfig : ScriptableObject
    {
        [Header("COLLISION CHECKING")]
        [Expandable] [SerializeField] private CollisionProbingConfig _floorCollisionProbing;
        
        [Header("DASH END POSITION")]
        [SerializeField, Range(0.0f, 5.0f)] private float _heightDistanceFromFloor = 2f;
        [SerializeField, Range(-5.0f, 5.0f)] private float _forwardDistanceFromClaw = -3.0f;

        [Header("USER INTERACTIONS")] 
        [SerializeField, Range(-1.0f, 1.0f)] private float _minDotToAcceptUser = 0.1f;
        [SerializeField, Range(0f, 5.0f)] private float _heightDistanceToAcceptUser = 1.5f; 
        
        public CollisionProbingConfig FloorCollisionProbingConfig => _floorCollisionProbing;

        public float HeightDistanceFromFloor => _heightDistanceFromFloor;
        public float ForwardDistanceFromClaw => _forwardDistanceFromClaw;


        public float MinDotToAcceptUser => _minDotToAcceptUser;
        public float HeightDistanceToAcceptUser => _heightDistanceToAcceptUser;


    }
}