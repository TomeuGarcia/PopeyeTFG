using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    
    [System.Serializable]
    public class AutoAimTargetResultFiltererConfig
    {
        [SerializeField, Range(0.0f, 90.0f)] private float _angularDistanceToDiscard = 5.0f;

        public float AngularDistanceToDiscard => _angularDistanceToDiscard;
        
        
        public AutoAimTargetResultFiltererConfig()
        {
            OnValidate();
        }
        
        public void OnValidate()
        {
            
        }
    }
}