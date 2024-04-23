using Popeye.ProjectHelpers;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    [System.Serializable]
    public class AutoAimTargetFinderConfig
    {
        [SerializeField, Range(0.0f, 30.0f)] private float _radiusDistance = 8.0f;

        public float RadiusDistance => _radiusDistance;
        
        public AutoAimTargetFinderConfig()
        {
            OnValidate();
        }
        
        public void OnValidate()
        {
            
        }
    }
}