using NaughtyAttributes;
using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "HangingPhysicsChainViewConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "HangingPhysicsChainViewConfig")]
    public class HangingPhysicsChainViewLogicConfig : ScriptableObject
    {
        [Header("COLLISIONS")]
        [Expandable]
        [SerializeField] private CollisionProbingConfig _collisionProbingConfig;
        
        
        [Header("HANGING")]
        [SerializeField, Range(0, 0.5f)] private float _verticalOffsetFromFloor = 0.01f;
        [SerializeField, Range(0, 20.0f)] private float _fullStraightDistance = 8.0f;
        
        [SerializeField] private AnimationCurve _bendingWeightCurve;

        
        public CollisionProbingConfig CollisionProbingConfig => _collisionProbingConfig;
        
        public float VerticalOffsetFromFloor => _verticalOffsetFromFloor;
        public float FullStraightDistance => _fullStraightDistance;
        
        
        public AnimationCurve BendingWeightCurve => _bendingWeightCurve;
    }
}