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
        [FormerlySerializedAs("_throwViewConfig")]
        [Space(20)]
        
        
        [Header("CONFIGURATIONS")]
        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig throwViewLogicConfig;
        [FormerlySerializedAs("_pullViewConfig")]
        [Space(20)]
        
        [Expandable]
        [SerializeField] private SpiralThrowChainViewLogicConfig pullViewLogicConfig;
        [FormerlySerializedAs("_restingOnFloorViewConfig")]
        [Space(20)]
        
        [Expandable]
        [SerializeField] private HangingPhysicsChainViewLogicConfig restingOnFloorViewLogicConfig;



        public int ChainBoneCount => _chainBoneCount;
        
        public SpiralThrowChainViewLogicConfig ThrowViewLogicConfig => throwViewLogicConfig;
        public SpiralThrowChainViewLogicConfig PullViewLogicConfig => pullViewLogicConfig;
        public HangingPhysicsChainViewLogicConfig RestingOnFloorViewLogicConfig => restingOnFloorViewLogicConfig;
    }
}