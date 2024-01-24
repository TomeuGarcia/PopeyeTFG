using System;
using Project.Scripts.Math.Functions;
using UnityEngine;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [ExecuteInEditMode]
    public class AutoAimFunction_Test : MonoBehaviour
    {
        [Header("WORLD")] 
        [SerializeField] private AutoAimWorld_Test _autoAimWorldTest;
        
        [Header("FUNCTION")] 
        [SerializeField] private Transform _functionOrigin;
        [SerializeField] private LineRenderer _orientationFunctionLine;
        [SerializeField, Range(10, 200)] private int _smoothCount = 100;

        [Header("Function Configuration")]
        [SerializeField, Range(0f, 1f)] private float _blendWithIdentity = 0f;
        
        [Header("TARGETER")] 
        [SerializeField] private Transform _lookRepresentation;
        
        private static readonly Vector2 FUNCTION_LINE_SIZE = new Vector2(10, 10);
        private const float ANGLE_LIMIT_DELTA = 0.01f;
        
        private Vector2[] _functionDataTable;
        private MonotoneCubicFunction _f;
        
        
        private void Update()
        {
            DoUpdate();
        }

        private void DoUpdate()
        {
            bool updateTargeterLook = _autoAimWorldTest.DoUpdate();
            
            UpdateFunctionDataTable();
            //Array.Sort(_functionDataTable, (a,b) => a.y < b.y ? -1 : 1);
            
            UpdateFunctionWithDataTable();

            if (updateTargeterLook) UpdateTargeterLook();
            
            DrawOrientationFunctionLine(_orientationFunctionLine, _f, _smoothCount);
        }


        private void UpdateFunctionDataTable()
        {
            AutoAimTargetData_Test[] autoAimTargets = _autoAimWorldTest.AimTargetsData;
            
            int numAngles = 2 + (autoAimTargets.Length * 6);
            _functionDataTable = new Vector2[numAngles];

            _functionDataTable[0] = Vector2.zero;
            _functionDataTable[^1] = Vector2.one * 360f;
            
            for (int i = 0; i < autoAimTargets.Length; ++i)
            {
                AutoAimTargetData_Test autoAimTargetData = autoAimTargets[i];
                
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
                _functionDataTable[index]     = new Vector2(angle_X_leftLimitOut, angle_Y_leftLimitOut);
                _functionDataTable[index + 1] = new Vector2(angle_X_leftLimitIn, angle_Y_leftLimitIn);
                _functionDataTable[index + 2] = new Vector2(angle_X_leftCenter, angle_X_center);
                _functionDataTable[index + 3] = new Vector2(angle_X_rightCenter, angle_X_center);
                _functionDataTable[index + 4] = new Vector2(angle_X_rightLimitIn, angle_Y_rightLimitIn);
                _functionDataTable[index + 5] = new Vector2(angle_X_rightLimitOut, angle_Y_rightLimitOut);
                
                

                // Draw
                autoAimTargetData.HelpViewerA.position = 
                    AnglesToDrawPosition(angle_X_leftLimitIn, angle_Y_leftLimitIn, 0.1f);
                autoAimTargetData.HelpViewer.position = 
                    AnglesToDrawPosition(angle_X_center, angle_X_center, 0f);
                autoAimTargetData.HelpViewerB.position = 
                    AnglesToDrawPosition(angle_X_rightLimitIn, angle_Y_rightLimitIn, 0.1f);
            }
        }

        private void UpdateFunctionWithDataTable()
        {
            _f = new MonotoneCubicFunction(_functionDataTable);
        }

        private void UpdateTargeterLook()
        {
            float lookX = _autoAimWorldTest.TargeterLookAngle;
            float lookY = EvaluateFunction(_f, lookX);
            _autoAimWorldTest.SetTargeterLookDirection(lookY);
            
            // Draw
            _lookRepresentation.position = AnglesToDrawPosition(lookX, lookY, 0.1f);
        }
        
        
        
        private void DrawOrientationFunctionLine(LineRenderer line, MonotoneCubicFunction f, int smoothCount)
        {
            line.positionCount = smoothCount;
            line.SetPosition(0, AnglesToDrawPosition(0, 0, 0.1f));
            
            float step = 360f / (smoothCount-1);
            for (int i = 1; i < smoothCount; ++i)
            {
                float x = step * i;
                float y = EvaluateFunction(f, x);
                Vector3 position = AnglesToDrawPosition(x, y, 0.1f) ;
                line.SetPosition(i, position);
            }
        }


        private Vector3 AnglesToDrawPosition(float angleX, float angleY, float drawOffset)
        {
            return FunctionPointToDrawPosition(AnglesToFunctionPoint(angleX, angleY), drawOffset);
        }
        
        private Vector2 AnglesToFunctionPoint(float angleX, float angleY)
        {
            Vector2 functionPoint = FUNCTION_LINE_SIZE;
            functionPoint.x *= angleX / 360f;
            functionPoint.y *= angleY / 360f;
            return functionPoint;
        }

        private Vector3 FunctionPointToDrawPosition(Vector2 functionPoint, float drawOffset)
        {
            return (Vector3.right * functionPoint.x) +
                   (Vector3.forward * functionPoint.y) +
                   (Vector3.up * drawOffset) +
                   _functionOrigin.position;
        }

        private float EvaluateFunction(MonotoneCubicFunction function, float x)
        {
            return Mathf.Lerp(function.Evaluate(x), x, _blendWithIdentity);
        }
    }
}