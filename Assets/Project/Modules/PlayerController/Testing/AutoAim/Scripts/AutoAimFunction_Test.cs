using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [ExecuteInEditMode]
    public class AutoAimFunction_Test : MonoBehaviour
    {
        [Button("Update")]
        private void UpdateButton() {DoUpdate();}

        [Header("WORLD")] 
        [SerializeField] private AutoAimWorld_Test _autoAimWorldTest;
        
        [Header("FUNCTION")] 
        [SerializeField] private Transform _functionOrigin;
        [SerializeField] private LineRenderer _orientationFunctionLine;

        private Vector2 _functionLineSize = new Vector2(10, 10);

        private float _angleToFunctionSize = 360f /10f;

        [SerializeField, Range(0f, 1f)] private float _flattening = 0f;
        
        private Vector2[] _functionPoints;

        [Header("TARGETER")] 
        [SerializeField] private Transform _lookRepresentation;
        
        
        private void Update()
        {
            DoUpdate();
        }

        private void DoUpdate()
        {
            _autoAimWorldTest.DoUpdate();
            
            
            AutoAimTargetData_Test[] autoAimTargets = _autoAimWorldTest.AimTargetsData;

            _functionPoints = new Vector2[2 + (autoAimTargets.Length * 5)];
            
            _functionPoints[0] = AngleToFunctionPoint(0, 0);
            _functionPoints[_orientationFunctionLine.positionCount-1] = AngleToFunctionPoint(360, 0);
            
            for (int i = 0; i < autoAimTargets.Length; ++i)
            {
                AutoAimTargetData_Test autoAimTargetData = autoAimTargets[i];
                Vector2 functionPosition = AngleToFunctionPoint(autoAimTargetData.AngularPosition, 0);

                float targetRegionAngularDifference =
                    autoAimTargetData.HalfAngularTargetRegion - autoAimTargetData.HalfAngularSize;
                
                float angleA = autoAimTargetData.AngularPosition - autoAimTargetData.HalfAngularSize;
                float angleB = autoAimTargetData.AngularPosition + autoAimTargetData.HalfAngularSize;
                Vector2 functionPositionA = AngleToFunctionPoint(angleA, -targetRegionAngularDifference);
                Vector2 functionPositionB = AngleToFunctionPoint(angleB, targetRegionAngularDifference);
                
                int index = 1 + (i * 5);
                _functionPoints[index]     = functionPositionA;
                _functionPoints[index + 1] = new Vector2(functionPositionA.x, Mathf.Lerp(functionPositionA.y, functionPosition.y, _flattening)); //
                _functionPoints[index + 2] = functionPosition;
                _functionPoints[index + 3] = new Vector2(functionPositionB.x, Mathf.Lerp(functionPositionB.y, functionPosition.y, _flattening)); //
                _functionPoints[index + 4] = functionPositionB;
                
                
                
                autoAimTargetData.HelpViewerA.position = FunctionPointToLinePosition(functionPositionA);
                autoAimTargetData.HelpViewer.position = FunctionPointToLinePosition(functionPosition);
                autoAimTargetData.HelpViewerB.position = FunctionPointToLinePosition(functionPositionB);
            }


            UpdateOrientationFunctionLine(_functionPoints);

            _lookRepresentation.position =
                FunctionPointToLinePosition(Sample(_autoAimWorldTest.TargeterLookAngle));
        }

        private void UpdateOrientationFunctionLine(Vector2[] points)
        {
            _orientationFunctionLine.positionCount = points.Length;
            for (int i = 0; i < _orientationFunctionLine.positionCount; ++i)
            {
                _orientationFunctionLine.SetPosition(i, FunctionPointToLinePosition(points[i]));
            }
        }
        
        private Vector2 AngleToFunctionPoint(float angle, float YtoXScale)
        {
            Vector2 functionPoint = _functionLineSize;
            functionPoint.x *= (angle + YtoXScale) / 360f;
            functionPoint.y *= angle / 360f;
            return functionPoint;
        }

        private Vector3 FunctionPointToLinePosition(Vector2 functionPoint)
        {
            return (Vector3.right * functionPoint.x) + 
                   (Vector3.forward * functionPoint.y) + 
                   _functionOrigin.position;
        }


        private Vector2 Sample(float x)
        {
            
            return AngleToFunctionPoint(x, 0);
        }
    }
}