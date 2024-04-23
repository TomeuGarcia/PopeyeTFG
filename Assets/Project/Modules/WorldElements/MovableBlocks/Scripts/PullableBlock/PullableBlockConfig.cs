using Popeye.ProjectHelpers;
using UnityEngine;

namespace Project.Modules.WorldElements.MovableBlocks.PullableBlocks
{
    [CreateAssetMenu(fileName = "PullableBlockConfig", 
        menuName = ScriptableObjectsHelper.GRIDMOVEMENT_ASSETS_PATH + "PullableBlockConfig")]
    public class PullableBlockConfig : ScriptableObject
    {
        [SerializeField] private PullableBlockViewConfig _viewConfig;
        
        
        public PullableBlockViewConfig ViewConfig => _viewConfig;

    }
}