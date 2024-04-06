using Popeye.Modules.VFX.Anchor.Generic;
using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerAnchor.Anchor
{
    [CreateAssetMenu(fileName = "GeneralAnchorViewConfig", 
        menuName = ScriptableObjectsHelper.ANCHORVIEW_ASSETS_PATH + "GeneralAnchorViewConfig")]
    public class GeneralAnchorViewConfig : ScriptableObject
    {
        [Header("STRETCH")] 
        [SerializeField] private StretchAnchorViewConfig _stretchViewConfig;
        
        
        [Header("VFX")] 
        [SerializeField] private VFXAnchorViewConfig _vfxViewConfig;
        
        
        public VFXAnchorViewConfig VfxViewConfig => _vfxViewConfig;
        public StretchAnchorViewConfig StretchViewConfig => _stretchViewConfig;
    }
}
