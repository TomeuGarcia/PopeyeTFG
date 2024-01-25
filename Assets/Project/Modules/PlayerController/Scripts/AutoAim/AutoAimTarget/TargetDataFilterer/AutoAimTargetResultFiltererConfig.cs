using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    
    [CreateAssetMenu(fileName = "AutoAimTargetResultFiltererConfig", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimTargetResultFiltererConfig")]
    public class AutoAimTargetResultFiltererConfig : ScriptableObject
    {
        [SerializeField, Range(0.0f, 90.0f)] private float _angularDistanceToDiscard = 5.0f;

        public float AngularDistanceToDiscard => _angularDistanceToDiscard;
    }
}