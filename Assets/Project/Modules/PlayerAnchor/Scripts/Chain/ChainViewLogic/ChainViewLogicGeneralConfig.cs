using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    
    [CreateAssetMenu(fileName = "ChainViewGeneralConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "ChainViewGeneralConfig")]
    public class ChainViewLogicGeneralConfig : ScriptableObject
    {
        [Header("CHAIN BONES")]
        [SerializeField, Range(2, 100)] private int _chainBoneCount = 20;
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
        [SerializeField] private FoldingChainViewLogicConfig _dashingTowardsViewLogicConfig;
        [Space(20)]

        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig _dashingAwayViewLogicConfig;
        


        public int ChainBoneCount => _chainBoneCount;
        
        public SpiralThrowChainViewLogicConfig ThrowViewLogicConfig => _throwViewLogicConfig;
        public SpiralThrowChainViewLogicConfig PullViewLogicConfig => _pullViewLogicConfig;
        public BoneChainChainViewLogicConfig RestingOnFloorViewLogicConfig => _restingOnFloorViewLogicConfig;
        public FoldingChainViewLogicConfig DashingTowardsViewLogicConfig => _dashingTowardsViewLogicConfig;
        public SpiralThrowChainViewLogicConfig DashingAwayViewLogicConfig => _dashingAwayViewLogicConfig;
    }
}