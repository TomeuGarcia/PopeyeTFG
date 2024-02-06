using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Chain
{
    
    [CreateAssetMenu(fileName = "FoldingChainViewLogicConfig", 
        menuName = ScriptableObjectsHelper.ANCHORCHAIN_ASSETS_PATH + "FoldingChainViewLogicConfig")]
    public class FoldingChainViewLogicConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 0.99f)] private float _phaseOffset = 0.1f;
        [SerializeField] private AnimationCurve _phaseWeightCurve = AnimationCurve.Linear(0,1,1,1);

        public float PhaseOffset => _phaseOffset;
        public AnimationCurve PhaseWeightCurve => _phaseWeightCurve;
    }
}