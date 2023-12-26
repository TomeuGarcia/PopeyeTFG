using Project.Modules.PlayerAnchor.Chain;
using Project.Scripts.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.PlayerAnchor.Anchor.AnchorConfigurations
{
    [CreateAssetMenu(fileName = "AnchorGeneralConfig", 
        menuName = ScriptableObjectsHelper.ANCHOR_ASSETS_PATH + "AnchorGeneralConfig")]
    public class AnchorGeneralConfig : ScriptableObject
    {
        [Header("OTHER CONFIGURATIONS")]
        [SerializeField] private ChainConfig _anchorChainConfig;
        [SerializeField] private AnchorDamageConfig _anchorDamageConfig;
        [SerializeField] private AnchorMotionConfig _anchorMotionConfig;
        [SerializeField] private AnchorThrowConfig _anchorThrowConfig;
        [SerializeField] private AnchorThrowConfig _anchorVerticalThrowConfig;
        [SerializeField] private AnchorPullConfig _anchorPullConfig;
        [SerializeField] private AnchorKickConfig _anchorKickConfig;
        [SerializeField] private AnchorSpinConfig _anchorSpinConfig;
        
        public ChainConfig ChainConfig => _anchorChainConfig;
        public AnchorDamageConfig DamageConfig => _anchorDamageConfig;
        public AnchorMotionConfig MotionConfig => _anchorMotionConfig;
        public AnchorThrowConfig ThrowConfig => _anchorThrowConfig;
        public AnchorThrowConfig VerticalThrowConfig => _anchorVerticalThrowConfig;
        public AnchorPullConfig PullConfig => _anchorPullConfig;
        public AnchorKickConfig KickConfig => _anchorKickConfig;
        public AnchorSpinConfig SpinConfig => _anchorSpinConfig;
    }
}