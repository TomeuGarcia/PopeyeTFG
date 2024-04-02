using Popeye.ProjectHelpers;
using Project.Scripts.TweenExtensions;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "StretchAnchorViewConfig", 
        menuName = ScriptableObjectsHelper.ANCHORVIEW_ASSETS_PATH + "StretchAnchorViewConfig")]
    public class StretchAnchorViewConfig : ScriptableObject
    {
        [Header("THROW")] 
        [SerializeField] private TweenPunchConfig _scaleThrowPunchConfig;
        public TweenPunchConfig ScaleThrowPunchConfig => _scaleThrowPunchConfig;
        
        
        [Header("PULL")] 
        [SerializeField] private TweenPunchConfig _scalePullPunchConfig;
        public TweenPunchConfig ScalePullPunchConfig => _scalePullPunchConfig;
        
        
        [Header("PICK UP")] 
        [SerializeField] private TweenPunchConfig _scaleCarriedPunchConfig;
        public TweenPunchConfig ScaleCarriedPunchConfig => _scaleCarriedPunchConfig;
        
        
        [Header("VERTICAL HIT")] 
        [SerializeField] private TweenPunchConfig _scaleVerticalHitPunchConfig;
        public TweenPunchConfig ScaleVerticalHitPunchConfig => _scaleVerticalHitPunchConfig;
    }
}