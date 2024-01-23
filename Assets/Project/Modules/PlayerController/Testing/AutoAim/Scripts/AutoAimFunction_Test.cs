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
        [SerializeField] private LineRenderer _orientationFunctionLine_Smooth;

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

            int numPoints = 2 + (autoAimTargets.Length * 5);
            _functionPoints = new Vector2[numPoints];
            int numAngles = 2 + (autoAimTargets.Length * 3);
            Vector2[] anglesXY = new Vector2[numAngles];
            
            _functionPoints[0] = AngleToFunctionPoint(0, 0, out float angleX);
            _functionPoints[^1] = AngleToFunctionPoint(360, 0, out angleX);
            anglesXY[0] = Vector2.zero;
            anglesXY[^1] = Vector2.one * 360f;
            
            for (int i = 0; i < autoAimTargets.Length; ++i)
            {
                AutoAimTargetData_Test autoAimTargetData = autoAimTargets[i];
                Vector2 functionPosition = AngleToFunctionPoint(autoAimTargetData.AngularPosition, 0, out angleX);

                float targetRegionAngularDifference =
                    autoAimTargetData.HalfAngularTargetRegion - autoAimTargetData.HalfAngularSize;
                
                float angleA = autoAimTargetData.AngularPosition - autoAimTargetData.HalfAngularSize;
                float angleB = autoAimTargetData.AngularPosition + autoAimTargetData.HalfAngularSize;
                Vector2 functionPositionA = AngleToFunctionPoint(angleA, -targetRegionAngularDifference, out float angleAX);
                Vector2 functionPositionB = AngleToFunctionPoint(angleB, targetRegionAngularDifference, out float angleBX);

                functionPositionA.y = Mathf.Lerp(functionPositionA.y, functionPosition.y, _flattening);
                functionPositionB.y = Mathf.Lerp(functionPositionB.y, functionPosition.y, _flattening);


                angleA = Mathf.Lerp(angleA, autoAimTargetData.AngularPosition, _flattening);
                angleB = Mathf.Lerp(angleB, autoAimTargetData.AngularPosition, _flattening);

                float angleDelta = 0.01f;
                float angleA2 = angleA - angleDelta;
                float angleB2 = angleB + angleDelta;
                Vector2 functionPositionA2 = AngleToFunctionPoint(angleA2, -targetRegionAngularDifference, out float angleA2X);
                Vector2 functionPositionB2 = AngleToFunctionPoint(angleB2, targetRegionAngularDifference, out float angleB2X);
                
                int index = 1 + (i * 5);
                _functionPoints[index]     = functionPositionA2;
                _functionPoints[index + 1] = functionPositionA; //
                _functionPoints[index + 2] = functionPosition;
                _functionPoints[index + 3] = functionPositionB; //
                _functionPoints[index + 4] = functionPositionB2;
                
                

                index = 1 + (i * 3);
                //anglesXY[index]     = new Vector2(angleA2X, angleA2);
                anglesXY[index]     = new Vector2(angleAX, angleA);
                anglesXY[index + 1] = new Vector2(angleX, autoAimTargetData.AngularPosition);
                anglesXY[index + 2] = new Vector2(angleBX, angleB);
                //anglesXY[index + 4] = new Vector2(angleB2X, angleB2);

                autoAimTargetData.HelpViewerA.position = FunctionPointToLinePosition(functionPositionA);
                autoAimTargetData.HelpViewer.position = FunctionPointToLinePosition(functionPosition);
                autoAimTargetData.HelpViewerB.position = FunctionPointToLinePosition(functionPositionB);
            }


            UpdateOrientationFunctionLine(_functionPoints, anglesXY);
        }

        private void UpdateOrientationFunctionLine(Vector2[] points, Vector2[] angles)
        {
            _orientationFunctionLine.positionCount = points.Length;
            for (int i = 0; i < _orientationFunctionLine.positionCount; ++i)
            {
                _orientationFunctionLine.SetPosition(i, FunctionPointToLinePosition(points[i]));
            }

            int smoothCount = 100;
            _orientationFunctionLine_Smooth.positionCount = smoothCount;
            MathematicalFunction_Test f = new MathematicalFunction_Test(angles);
            
            float step = 360f / (smoothCount-1);
            for (int i = 1; i < smoothCount; ++i)
            {
                float x = step * i;
                float y = f.Evaluate(x);
                Vector3 position = FunctionPointToLinePosition(AnglesToFunctionPoint(x, y)) + Vector3.up * 0.2f;
                _orientationFunctionLine_Smooth.SetPosition(i, position);
            }
            _orientationFunctionLine_Smooth.SetPosition(0, FunctionPointToLinePosition(Vector2.zero));
            
            
            
            float lookX = _autoAimWorldTest.TargeterLookAngle;
            float lookY = f.Evaluate(lookX);
            _lookRepresentation.position = FunctionPointToLinePosition(AnglesToFunctionPoint(lookX, lookY)) + Vector3.up * 0.2f;

            _autoAimWorldTest.SetTargeterLookDirection(lookY);
        }

        private Vector2 AnglesToFunctionPoint(float angleX, float angleY)
        {
            Vector2 functionPoint = _functionLineSize;
            functionPoint.x *= angleX / 360f;
            functionPoint.y *= angleY / 360f;
            return functionPoint;
        }
        private Vector2 AngleToFunctionPoint(float angleY, float YtoXScale, out float angleX)
        {
            Vector2 functionPoint = _functionLineSize;
            angleX = angleY + YtoXScale;
            functionPoint.x *= angleX / 360f;
            functionPoint.y *= angleY / 360f;
            return functionPoint;
        }

        private Vector3 FunctionPointToLinePosition(Vector2 functionPoint)
        {
            return (Vector3.right * functionPoint.x) + 
                   (Vector3.forward * functionPoint.y) + 
                   _functionOrigin.position;
        }
    }
}