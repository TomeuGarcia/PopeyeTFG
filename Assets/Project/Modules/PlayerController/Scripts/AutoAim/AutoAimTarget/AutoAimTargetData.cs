using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimTargetData
    {
        private IAutoAimTarget _autoAimTarget;

        public GameObject GameObject => _autoAimTarget.GameObject;
        public float AngularPosition { get; private set; }
        
        public float HalfAngularSize => _autoAimTarget.DataConfig.HalfAngularSize;
        public float HalfAngularTargetRegion => _autoAimTarget.DataConfig.HalfAngularTargetRegion;
        public float HalfFlatCenterAngularTargetRegion => _autoAimTarget.DataConfig.HalfFlatCenterAngularTargetRegion;
        

        public void Configure(IAutoAimTarget autoAimTarget, float angularPosition)
        {
            _autoAimTarget = autoAimTarget;
            AngularPosition = angularPosition;
        }
        
        
    }
}