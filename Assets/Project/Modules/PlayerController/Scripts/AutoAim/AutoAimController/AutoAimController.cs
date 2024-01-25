
using Project.Scripts.Core.DataStructures;
using Project.Scripts.Math.Functions;
using UnityEngine;

namespace Popeye.Modules.PlayerController.AutoAim
{
    public class AutoAimController
    {
        private const float ANGLE_LIMIT_DELTA = 0.01f;
        
        private AutoAimControllerConfig _config;
        private AutoAimTargetsController _autoAimTargetsController;
        
        private ArrayBuffer<Vector2> _functionDataTable;
        public MonotoneCubicFunction OrientationRemapFunction { get; private set; }
        public AutoAimTargetData[] AutoAimTargetsData { get; private set; }

        public AutoAimControllerConfig Config => _config;


        public void Configure(AutoAimControllerConfig config, AutoAimTargetsController autoAimTargetsController)
        {
            _config = config;
            _autoAimTargetsController = autoAimTargetsController;
            
            _functionDataTable = new ArrayBuffer<Vector2>(_config.MaxDataCapacity);
            OrientationRemapFunction = new MonotoneCubicFunction();
        }
        
        public float CorrectLookAngle(float lookAngle)
        {
            if (!_autoAimTargetsController.Update())
            {
                return lookAngle;
            }
            
            UpdateDataTable();
            UpdateFunctionWithDataTable();

            return RemapLookAngle(lookAngle);
        }

        private void UpdateDataTable()
        {
            AutoAimTargetsData = _autoAimTargetsController.GetAimTargetsData();
            
            int numAngles = 2 + (AutoAimTargetsData.Length * 6);
            
            _functionDataTable.ClearAndResize(numAngles);

            _functionDataTable.Elements[0] = Vector2.zero;
            _functionDataTable.Elements[numAngles - 1] = Vector2.one * 360f;
            
            for (int i = 0; i < AutoAimTargetsData.Length; ++i)
            {
                AutoAimTargetData autoAimTargetData = AutoAimTargetsData[i];
                
                // Center
                float angle_X_center = autoAimTargetData.AngularPosition;
                float angle_X_leftCenter = angle_X_center - autoAimTargetData.HalfFlatCenterAngularTargetRegion;
                float angle_X_rightCenter = angle_X_center + autoAimTargetData.HalfFlatCenterAngularTargetRegion;
                
                float targetRegionAngularDifference =
                    autoAimTargetData.HalfAngularTargetRegion - autoAimTargetData.HalfAngularSize;
                
                
                // Limit in
                float angle_Y_leftLimitIn = autoAimTargetData.AngularPosition - autoAimTargetData.HalfAngularSize;
                float angle_Y_rightLimitIn = autoAimTargetData.AngularPosition + autoAimTargetData.HalfAngularSize;
                
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
                
                /*
                // Draw
                autoAimTargetData.HelpViewerA.position = 
                    AnglesToDrawPosition(angle_X_leftLimitIn, angle_Y_leftLimitIn, 0.1f);
                autoAimTargetData.HelpViewer.position = 
                    AnglesToDrawPosition(angle_X_center, angle_X_center, 0f);
                autoAimTargetData.HelpViewerB.position = 
                    AnglesToDrawPosition(angle_X_rightLimitIn, angle_Y_rightLimitIn, 0.1f);
                */
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