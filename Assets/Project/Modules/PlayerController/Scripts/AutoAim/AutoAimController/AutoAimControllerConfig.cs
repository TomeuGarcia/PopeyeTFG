using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    [CreateAssetMenu(fileName = "AutoAimControllerConfig", 
        menuName = ScriptableObjectsHelper.AUTOAIM_ASSETS_PATH + "AutoAimControllerConfig")]
    public class AutoAimControllerConfig : ScriptableObject
    {
        [SerializeField, Range(50, 500)] private int _maxDataCapacity = 300;
        [SerializeField, Range(0f, 1f)] private float _blendWithIdentity = 0.0f;
        
        public int MaxDataCapacity => _maxDataCapacity;
        public float BlendWithIdentity => _blendWithIdentity;
    }
}