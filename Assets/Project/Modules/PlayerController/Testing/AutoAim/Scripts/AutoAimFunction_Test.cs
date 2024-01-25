using System;
using System.Buffers;
using System.Collections.Generic;
using Popeye.Modules.PlayerController.AutoAim;
using Project.Scripts.Core.DataStructures;
using Project.Scripts.Math.Functions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Modules.PlayerController.Testing.AutoAim.Scripts
{
    [ExecuteInEditMode]
    public class AutoAimFunction_Test : MonoBehaviour
    {
        [Header("WORLD")] 
        [SerializeField] private bool _findTargetsWithPhysics = false;
        [SerializeField] private AutoAimWorld_Test _autoAimWorldTest;
        
        [Header("FUNCTION")] 
        [SerializeField] private Transform _functionOrigin;
        [SerializeField] private LineRenderer _orientationFunctionLine;
        [SerializeField, Range(10, 200)] private int _smoothCount = 100;
        
        [Header("TARGETER")] 
        [SerializeField] private Transform _lookRepresentation;
        
        private static readonly Vector2 FUNCTION_LINE_SIZE = new Vector2(10, 10);

        [SerializeField] private AutoAimCreator _autoAimCreator_Physics;
        [SerializeField] private AutoAimCreator_Test _autoAimCreator_References;
        private AutoAimController _autoAimController;


        private Transform Targeter => _autoAimWorldTest.Targeter;

        private void Start()
        {
            InitAutoAimController();
        }

        private void InitAutoAimController()
        {
            _autoAimController =  _findTargetsWithPhysics ?
                _autoAimCreator_Physics.Create(Targeter) :
                _autoAimCreator_References.Create(Targeter, _autoAimWorldTest.AimTargetsParent);
        }
        

        private void Update()
        {
            if (_autoAimController == null)
            {
                InitAutoAimController();
            }
            
            DoUpdate();
        }

        private void DoUpdate()
        {
            // Update
            float lookX = 0, lookY = 0;
            bool lookChanged = _autoAimWorldTest.DoUpdate();
            lookX = _autoAimWorldTest.TargeterLookAngle;
            lookY = _autoAimController.CorrectLookAngle(lookX, Vector3.forward, Vector3.right);
            
            if (lookChanged)
            {
                _autoAimWorldTest.SetTargeterLookDirection(lookY);
            }

            // Draw
            DrawLookRepresentation(lookX, lookY);
            DrawOrientationFunctionLine(_orientationFunctionLine, _autoAimController.OrientationRemapFunction, _smoothCount);
            DrawAutoAimTargets();
        }


        private void DrawLookRepresentation(float lookX, float lookY)
        {
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

        private void DrawAutoAimTargets()
        {
            AutoAimTargetResult[] autoAimTargetDatas = _autoAimController.AutoAimTargetsData;
            AutoAimTargetData_Test[] autoAimTargetDataTests = _autoAimWorldTest.AimTargetsData;
            
            foreach (var autoAimTargetDataTest in autoAimTargetDataTests)
            {
                int dataTestIndex = -1;
                for (int i = 0; i < autoAimTargetDatas.Length; ++i)
                {
                    if (autoAimTargetDataTest.GameObject == autoAimTargetDatas[i].GameObject)
                    {
                        dataTestIndex = i;
                        break;
                    }
                }
                
                
                if (dataTestIndex == -1)
                {
                    autoAimTargetDataTest.IsNotBeingTargeted();
                    continue;
                }
                
                
                autoAimTargetDataTest.IsBeingTargeted();

                AutoAimTargetData_Test autoAimTargetData = autoAimTargetDataTest.GameObject.GetComponent<AutoAimTargetData_Test>();
                
                float angularPosition = autoAimTargetDatas[dataTestIndex].AngularPosition;
                float angle_X_center = angularPosition;
                
                float targetRegionAngularDifference =
                    autoAimTargetData.HalfAngularTargetRegion - autoAimTargetData.HalfAngularSize;
                // Limit in
                float angle_Y_leftLimitIn = angularPosition - autoAimTargetData.HalfAngularSize;
                float angle_Y_rightLimitIn = angularPosition + autoAimTargetData.HalfAngularSize;
                
                float angle_X_leftLimitIn = angle_Y_leftLimitIn - targetRegionAngularDifference;
                float angle_X_rightLimitIn = angle_Y_rightLimitIn + targetRegionAngularDifference;
                
                
                autoAimTargetData.HelpViewerA.position = 
                    AnglesToDrawPosition(angle_X_leftLimitIn, angle_Y_leftLimitIn, 0.1f);
                autoAimTargetData.HelpViewer.position = 
                    AnglesToDrawPosition(angle_X_center, angle_X_center, 0f);
                autoAimTargetData.HelpViewerB.position = 
                    AnglesToDrawPosition(angle_X_rightLimitIn, angle_Y_rightLimitIn, 0.1f);
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
            return Mathf.Lerp(function.Evaluate(x), x, _autoAimController.Config.BlendWithIdentity);
        }
    }
}