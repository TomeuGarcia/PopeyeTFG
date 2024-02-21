using NaughtyAttributes;
using Popeye.InverseKinematics.Bones;
using Popeye.ProjectHelpers;
using Popeye.Scripts.Collisions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    
    [CreateAssetMenu(fileName = "ChainViewGeneralConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "ChainViewGeneralConfig")]
    public class ChainViewLogicGeneralConfig : ScriptableObject
    {
        [Header("CHAIN")]
        [SerializeField, Range(2, 100)] private int _chainBoneCount = 20;
        [SerializeField] private ChainConfig _chainConfig;
        [SerializeField] private Bone _bonePrefab;
        [SerializeField] private Bone _boneEndEffectorPrefab;
        [SerializeField] private Material _boneSharedMaterial;
        [Space(20)]
        
        
        [Header("COLLISIONS")]
        [Expandable]
        [SerializeField] private CollisionProbingConfig _obstacleCollisionProbingConfig;
        [Space(20)]
        
        
        [Header("CONFIGURATIONS")]
        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig _throwViewLogicConfig;
        [Space(20)]
        
        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig _pullViewLogicConfig;
        [Space(20)]

        [Expandable]
        [SerializeField] private BoneChainChainViewLogicConfig _restingOnFloorViewLogicConfig;
        [Space(20)]

        [Expandable]
        [SerializeField] private FoldingChainViewLogicConfig _pickedUpTowardsViewLogicConfig;
        [Space(20)]

        [Expandable]
        [SerializeField] private FoldingChainViewLogicConfig _dashingTowardsViewLogicConfig;
        [Space(20)]

        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig _dashingAwayViewLogicConfig;
        


        public int ChainBoneCount => _chainBoneCount;
        public float MaxChainLength => _chainConfig.MaxChainLength;

        public Bone BonePrefab => _bonePrefab;
        public Bone BoneEndEffectorPrefab => _boneEndEffectorPrefab;
        public Material BoneSharedMaterial => _boneSharedMaterial;
        
        
        public CollisionProbingConfig ObstacleCollisionProbingConfig => _obstacleCollisionProbingConfig;
        
        
        public SpiralThrowChainViewLogicConfig ThrowViewLogicConfig => _throwViewLogicConfig;
        public SpiralThrowChainViewLogicConfig PullViewLogicConfig => _pullViewLogicConfig;
        public BoneChainChainViewLogicConfig RestingOnFloorViewLogicConfig => _restingOnFloorViewLogicConfig;
        public FoldingChainViewLogicConfig PickedUpTowardsViewLogicConfig => _pickedUpTowardsViewLogicConfig;
        public FoldingChainViewLogicConfig DashingTowardsViewLogicConfig => _dashingTowardsViewLogicConfig;
        public SpiralThrowChainViewLogicConfig DashingAwayViewLogicConfig => _dashingAwayViewLogicConfig;

        
        public void ApplySharedMaterialToBonePrefabs()
        {
            if (_boneSharedMaterial)
            {
                if (_bonePrefab)
                {
                    _bonePrefab.SetMaterial(_boneSharedMaterial);
                }
                if (_boneEndEffectorPrefab)
                {
                    _boneEndEffectorPrefab.SetMaterial(_boneSharedMaterial);
                }
            }
        }
    }
}