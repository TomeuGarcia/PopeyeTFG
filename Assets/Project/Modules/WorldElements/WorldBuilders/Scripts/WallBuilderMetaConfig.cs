using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    [CreateAssetMenu(fileName = "WallBuilderMetaConfig", 
        menuName = ScriptableObjectsHelper.WALLBUILDER_ASSETS_PATH + "WallBuilderMetaConfig")]
    public class WallBuilderMetaConfig : ScriptableObject
    {
        [SerializeField] private bool _onPlayDestroyWallBuilders = false;
        public bool DestroyOnPlay => _onPlayDestroyWallBuilders;
    }
}