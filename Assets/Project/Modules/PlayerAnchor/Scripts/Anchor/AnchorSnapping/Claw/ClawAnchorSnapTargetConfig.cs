using NaughtyAttributes;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "ClawAnchorSnapTargetConfig", 
        menuName = ScriptableObjectsHelper.SNAPTARGETS_ASSETS_PATH + "ClawAnchorSnapTargetConfig")]
    public class ClawAnchorSnapTargetConfig : ScriptableObject
    {
        [Header("VIEW")] 
        [Expandable] [SerializeField] private ClawAnchorSnapTargetViewConfig _viewConfig;
        public ClawAnchorSnapTargetViewConfig ViewConfig => _viewConfig;
        
        
        [Header("COLLISION CHECKING")]
        [Expandable] [SerializeField] private CollisionProbingConfig _floorCollisionProbing;
        
        [Header("DASH END POSITION")]
        [SerializeField, Range(0.0f, 5.0f)] private float _heightDistanceFromFloor = 2f;
        [SerializeField, Range(-5.0f, 5.0f)] private float _forwardDistanceFromClaw = -3.0f;

        [Header("USER INTERACTIONS")] 
        [SerializeField, Range(0.0f, 180.0f)] private float _minAngleToAcceptUser = 85.0f;
        [SerializeField, Range(0f, 5.0f)] private float _heightDistanceToAcceptUser = 1.5f; 
        
        public CollisionProbingConfig FloorCollisionProbingConfig => _floorCollisionProbing;

        public float HeightDistanceFromFloor => _heightDistanceFromFloor;
        public float ForwardDistanceFromClaw => _forwardDistanceFromClaw;


        public float MinDotToAcceptUser { get; private set; }
        public float HeightDistanceToAcceptUser => _heightDistanceToAcceptUser;


        private void OnValidate()
        {
            MinDotToAcceptUser = Mathf.Cos(_minAngleToAcceptUser * Mathf.Rad2Deg);
        }
    }
}