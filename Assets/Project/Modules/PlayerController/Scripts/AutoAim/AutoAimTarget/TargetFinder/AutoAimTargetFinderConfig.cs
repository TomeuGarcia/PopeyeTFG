using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    
    [CreateAssetMenu(fileName = "AutoAimTargetFinderConfig", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimTargetFinderConfig")]
    public class AutoAimTargetFinderConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 30.0f)] private float _radiusDistance = 8.0f;

        public float RadiusDistance => _radiusDistance;
    }
}