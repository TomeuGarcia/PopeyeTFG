using Popeye.Modules.PlayerAnchor.Anchor.AnchorConfigurations;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    [CreateAssetMenu(fileName = "HangingPhysicsChainViewConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "HangingPhysicsChainViewConfig")]
    public class HangingPhysicsChainViewConfig : ScriptableObject
    {
        [Header("COLLISIONS")]
        [SerializeField] private CollisionProbingConfig _collisionProbingConfig;

        [Header("CHAIN BONES")]
        [SerializeField, Range(2, 100)] private int _chainBoneCount = 50;
        
        
        [Header("HANGING")]
        [SerializeField, Range(0, 0.5f)] private float _verticalOffsetFromFloor = 0.01f;
        [SerializeField, Range(0, 20.0f)] private float _fullStraightDistance = 8.0f;
        
        [SerializeField] private AnimationCurve _bendingWeightCurve;

        
        public CollisionProbingConfig CollisionProbingConfig => _collisionProbingConfig;
        
        public int ChainBoneCount => _chainBoneCount;
        public float VerticalOffsetFromFloor => _verticalOffsetFromFloor;
        public float FullStraightDistance => _fullStraightDistance;
        
        
        public AnimationCurve BendingWeightCurve => _bendingWeightCurve;
    }
}