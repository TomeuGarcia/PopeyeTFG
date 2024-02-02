
using Project.Scripts.Core.DataStructures;
using Project.Scripts.Math.Functions;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimController
    {
        private const float ANGLE_LIMIT_DELTA = 0.01f;
        
        private AutoAimFunctionConfig _config;
        private AutoAimTargetingController _autoAimTargetingController;
        
        private ArrayBuffer<Vector2> _functionDataTable;
        public MonotoneCubicFunction OrientationRemapFunction { get; private set; }
        public AutoAimTargetResult[] AutoAimTargetsData { get; private set; }

        public AutoAimFunctionConfig Config => _config;


        public void Configure(AutoAimFunctionConfig config, AutoAimTargetingController autoAimTargetingController)
        {
            _config = config;
            _autoAimTargetingController = autoAimTargetingController;
            
            _functionDataTable = new ArrayBuffer<Vector2>(_config.MaxDataCapacity);
            OrientationRemapFunction = new MonotoneCubicFunction();
        }
        
        public float CorrectLookAngle(float lookAngle, Vector3 forwardDirection, Vector3 rightDirection)
        {
            if (!_autoAimTargetingController.Update(forwardDirection, rightDirection))
            {
                return lookAngle;
            }
            
            UpdateDataTable();
            UpdateFunctionWithDataTable();

            return RemapLookAngle(lookAngle);
        }

        private void UpdateDataTable()
        {
            AutoAimTargetsData = _autoAimTargetingController.GetAimTargetsData();
            
            int numAngles = 2 + (AutoAimTargetsData.Length * 6);
            
            _functionDataTable.ClearAndResize(numAngles);

            _functionDataTable.Elements[0] = Vector2.zero;
            _functionDataTable.Elements[numAngles - 1] = Vector2.one * 360f;
            
            for (int i = 0; i < AutoAimTargetsData.Length; ++i)
            {
                AutoAimTargetResult autoAimTargetResult = AutoAimTargetsData[i];
                
                // Center
                float angle_X_center = autoAimTargetResult.AngularPosition;
                float angle_X_leftCenter = angle_X_center - autoAimTargetResult.HalfFlatCenterAngularTargetRegion;
                float angle_X_rightCenter = angle_X_center + autoAimTargetResult.HalfFlatCenterAngularTargetRegion;
                
                float targetRegionAngularDifference =
                    autoAimTargetResult.HalfAngularTargetRegion - autoAimTargetResult.HalfAngularSize;
                
                
                // Limit in
                float angle_Y_leftLimitIn = autoAimTargetResult.AngularPosition - autoAimTargetResult.HalfAngularSize;
                float angle_Y_rightLimitIn = autoAimTargetResult.AngularPosition + autoAimTargetResult.HalfAngularSize;
                
                float angle_X_leftLimitIn = angle_Y_leftLimitIn - targetRegionAngularDifference;
                float angle_X_rightLimitIn = angle_Y_rightLimitIn + targetRegionAngularDifference;
                

                // Limit out
                float angle_Y_leftLimitOut = angle_Y_leftLimitIn - ANGLE_LIMIT_DELTA;
                float angle_Y_rightLimitOut = angle_Y_rightLimitIn + ANGLE_LIMIT_DELTA;
                
                float angle_X_leftLimitOut = angle_Y_leftLimitOut - targetRegionAngularDifference;
                float angle_X_rightLimitOut = angle_Y_rightLimitOut + targetRegionAngularDifference;


                
                int index = 1 + (i * 6);
                
                _functionDataTable.Elements[index].x     = angle_X_leftLimitOut;
                _functionDataTable.Elements[index].y     = angle_Y_leftLimitOut;
                
                _functionDataTable.Elements[index + 1].x = angle_X_leftLimitIn;
                _functionDataTable.Elements[index + 1].y = angle_Y_leftLimitIn;
                
                _functionDataTable.Elements[index + 2].x = angle_X_leftCenter;
                _functionDataTable.Elements[index + 2].y = angle_X_center;
                
                _functionDataTable.Elements[index + 3].x = angle_X_rightCenter;
                _functionDataTable.Elements[index + 3].y = angle_X_center;
                
                _functionDataTable.Elements[index + 4].x = angle_X_rightLimitIn;
                _functionDataTable.Elements[index + 4].y = angle_Y_rightLimitIn;
                
                _functionDataTable.Elements[index + 5].x = angle_X_rightLimitOut;
                _functionDataTable.Elements[index + 5].y = angle_Y_rightLimitOut;
            } 
        }
        
        private void UpdateFunctionWithDataTable()
        {
            OrientationRemapFunction.Update(_functionDataTable.ToArray());
        }
        
        private float RemapLookAngle(float lookAngle)
        {
            return EvaluateOrientationRemap(lookAngle);
        }
        
        private float EvaluateOrientationRemap(float x)
        {
            return Mathf.Lerp(OrientationRemapFunction.Evaluate(x), x, _config.BlendWithIdentity);
        }
    }
}