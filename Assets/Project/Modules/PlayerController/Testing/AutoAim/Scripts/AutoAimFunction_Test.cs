using System;
using NaughtyAttributes;
using UnityEngine;

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
        [SerializeField] private LineRenderer _functionLine;

        [SerializeField] private Vector3 _functionSize = new Vector3(10, 0, 10);

        private float _angleToFunctionSize = 360f /10f;

        [SerializeField] private float extra = 10;
        
        private Vector3[] points;
        
        
        private void Update()
        {
            DoUpdate();
        }

        private void DoUpdate()
        {
            _autoAimWorldTest.DoUpdate();
            
            
            AutoAimTargetData_Test[] autoAimTargets = _autoAimWorldTest.AimTargetsData;

            points = new Vector3[2 + (autoAimTargets.Length * 3)];
            
            points[0] = AngleToFunctionPoint(0, 0);
            points[_functionLine.positionCount-1] = AngleToFunctionPoint(360, 0);
            
            for (int i = 0; i < autoAimTargets.Length; ++i)
            {
                AutoAimTargetData_Test autoAimTargetData = autoAimTargets[i];
                Vector3 functionPosition = AngleToFunctionPoint(autoAimTargetData.AngleAtCenter, 0);

                float angleA = autoAimTargetData.AngleAtCenter - autoAimTargetData.HalfAngleSize;
                float angleB = autoAimTargetData.AngleAtCenter + autoAimTargetData.HalfAngleSize;
                Vector3 functionPositionA = AngleToFunctionPoint(angleA, -extra);
                Vector3 functionPositionB = AngleToFunctionPoint(angleB, extra);
                
                int index = 1 + (i * 3);
                points[index] = functionPositionA;
                points[index + 1] = functionPosition;
                points[index + 2] = functionPositionB;
                
                
                autoAimTargetData.HelpViewerA.position = functionPositionA;
                autoAimTargetData.HelpViewer.position = functionPosition;
                autoAimTargetData.HelpViewerB.position = functionPositionB;
            }

            _functionLine.positionCount = points.Length;
            _functionLine.SetPositions(points);
        }

        
        private Vector3 AngleToFunctionPoint(float angle, float YtoXRatio)
        {
            Vector3 functionSize = _functionSize;
            functionSize.x *= (angle + YtoXRatio) / 360f;
            functionSize.z *= angle / 360f;
            return functionSize + _functionOrigin.position;
        }


        
    }
}