using Popeye.ProjectHelpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    
    [CreateAssetMenu(fileName = "FoldingChainViewLogicConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "FoldingChainViewLogicConfig")]
    public class FoldingChainViewLogicConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 2.0f)] private float _durationMultiplier = 0.0f;
        
        [Header("CHAIN DISTANCE")] 
        [SerializeField, Range(-10.0f, 10.0f)] private float _extraChainDistance = 0.0f;
        
        public float DurationMultiplier => _durationMultiplier;
        public float ExtraChainDistance => _extraChainDistance;
    }
}