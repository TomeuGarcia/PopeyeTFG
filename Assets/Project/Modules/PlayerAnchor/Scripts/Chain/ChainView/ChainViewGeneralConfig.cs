using NaughtyAttributes;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    
    [CreateAssetMenu(fileName = "ChainViewGeneralConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "ChainViewGeneralConfig")]
    public class ChainViewGeneralConfig : ScriptableObject
    {
        [Header("CHAIN BONES")]
        [SerializeField, Range(2, 100)] private int _chainBoneCount = 20;
        [Space(20)]
        
        
        [Header("CONFIGURATIONS")]
        [Expandable]
        [SerializeField] private SpiralThrowChainViewConfig _throwViewConfig;
        [Space(20)]
        
        [Expandable]
        [SerializeField] private SpiralThrowChainViewConfig _pullViewConfig;
        [Space(20)]
        
        [Expandable]
        [SerializeField] private HangingPhysicsChainViewConfig _restingOnFloorViewConfig;



        public int ChainBoneCount => _chainBoneCount;
        
        public SpiralThrowChainViewConfig ThrowViewConfig => _throwViewConfig;
        public SpiralThrowChainViewConfig PullViewConfig => _pullViewConfig;
        public HangingPhysicsChainViewConfig RestingOnFloorViewConfig => _restingOnFloorViewConfig;
    }
}