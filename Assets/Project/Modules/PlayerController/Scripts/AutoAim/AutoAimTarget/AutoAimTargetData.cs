using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetData
    {
        private AutoAimTargetDataConfig _config;
        
        public Vector3 Position { get; private set; }
        public float AngularPosition { get; private set; }
        
        public float HalfAngularSize => _config.HalfAngularSize;
        public float HalfAngularTargetRegion => _config.HalfAngularTargetRegion;
        public float HalfFlatCenterAngularTargetRegion => _config.HalfFlatCenterAngularTargetRegion;
        

        public void Configure(AutoAimTargetDataConfig config, Vector3 position, float angularPosition)
        {
            _config = config;
            Position = position;
            AngularPosition = angularPosition;
        }
        
        
    }
}