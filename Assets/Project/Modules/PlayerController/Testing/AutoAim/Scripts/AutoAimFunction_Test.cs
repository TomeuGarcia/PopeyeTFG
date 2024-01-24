using System;
using System.Buffers;
using System.Collections.Generic;
using Popeye.Modules.PlayerController.AutoAim;
using Project.Scripts.Core.DataStructures;
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
        
        private ArrayBuffer<Vector2> _functionDataTable = new ArrayBuffer<Vector2>(300);
        private MonotoneCubicFunction _f = new MonotoneCubicFunction();

        [SerializeField] private AutoAimCreator _autoAimCreator;
        private AutoAimController _autoAimController;

        [SerializeField] private Transform _targeter;

        private void Start()
        {
            _autoAimController = _autoAimCreator.Create(_targeter, Vector3.forward, Vector3.right);
        }

        private void Update()
        {
            if (_autoAimController == null)
            {
                _autoAimController = _autoAimCreator.Create(_targeter, Vector3.forward, Vector3.right);
            }
            
            DoUpdate();
        }

        private void DoUpdate()
        {
            _autoAimWorldTest.DoUpdate();
            
            float lookX = _autoAimWorldTest.TargeterLookAngle;
            float lookY = _autoAimController.CorrectLookAngle(lookX);
            _autoAimWorldTest.SetTargeterLookDirection(lookY);
            
            // Draw
            _lookRepresentation.position = AnglesToDrawPosition(lookX, lookY, 0.1f);
            
            DrawOrientationFunctionLine(_orientationFunctionLine, _autoAimController.OrientationRemapFunction, _smoothCount);

            
            AutoAimTargetData[] autoAimTargetDatas = _autoAimController.AutoAimTargetsData;
            for (int i = 0; i < autoAimTargetDatas.Length; ++i)
            {
                AutoAimTargetData_Test autoAimTargetData = autoAimTargetDatas[i].GameObject.GetComponent<AutoAimTargetData_Test>();
                
                
                float angle_X_center = autoAimTargetData.AngularPosition;
                
                float targetRegionAngularDifference =
                    autoAimTargetData.HalfAngularTargetRegion - autoAimTargetData.HalfAngularSize;
                // Limit in
                float angle_Y_leftLimitIn = autoAimTargetData.AngularPosition - autoAimTargetData.HalfAngularSize;
                float angle_Y_rightLimitIn = autoAimTargetData.AngularPosition + autoAimTargetData.HalfAngularSize;
                
                float angle_X_leftLimitIn = angle_Y_leftLimitIn - targetRegionAngularDifference;
                float angle_X_rightLimitIn = angle_Y_rightLimitIn + targetRegionAngularDifference;
                
                autoAimTargetData.HelpViewerA.position = 
                    AnglesToDrawPosition(angle_X_leftLimitIn, angle_Y_leftLimitIn, 0.1f);
                autoAimTargetData.HelpViewer.position = 
                    AnglesToDrawPosition(angle_X_center, angle_X_center, 0f);
                autoAimTargetData.HelpViewerB.position = 
                    AnglesToDrawPosition(angle_X_rightLimitIn, angle_Y_rightLimitIn, 0.1f);
                    
            }


            return;
                
            
            bool updateTargeterLook = _autoAimWorldTest.DoUpdate();
            
            UpdateFunctionDataTable();
            
            UpdateFunctionWithDataTable();

            if (updateTargeterLook) UpdateTargeterLook();
            
            DrawOrientationFunctionLine(_orientationFunctionLine, _f, _smoothCount);
        }


        private void UpdateFunctionDataTable()
        {
            AutoAimTargetData_Test[] autoAimTargets = _autoAimWorldTest.AimTargetsData;
            
            int numAngles = 2 + (autoAimTargets.Length * 6);
            
            _functionDataTable.ClearAndResize(numAngles);

            _functionDataTable.Elements[0] = Vector2.zero;
            _functionDataTable.Elements[numAngles - 1] = Vector2.one * 360f;
            
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
            _f.Update(_functionDataTable.ToArray());
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