using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Project.Scripts.Math.Curves
{
    public class QuadraticBezierCurveBehaviour : MonoBehaviour
    {
        private QuadraticBezierCurve _curve;

        [SerializeField] private LineRenderer _line;
        [SerializeField, Range(1, 100)] private int _numberOfSegments = 50;    

        [SerializeField] private Transform _t0;
        [SerializeField] private Transform _t1;
        [SerializeField] private Transform _t2;
        [SerializeField] private Transform _t3;
    
        public Transform T0 => _t0;
        public Transform T1 => _t1;
        public Transform T2 => _t2;
        public Transform T3 => _t3;

        private void OnValidate()
        {
            if (HasAllReferences())
            {
                Awake();
                Update();
            }
        }

        private void Awake()
        {
            AwakeInit();
        }

        private void Update()
        {
            UpdatePositions();
            DrawLine();
        }


        private void AwakeInit()
        {
            _curve = new QuadraticBezierCurve(T0.position, T1.position, T2.position, T3.position);
        }

        private void UpdatePositions()
        {
            _curve.P0 = T0.position;
            _curve.P1 = T1.position;
            _curve.P2 = T2.position;
            _curve.P3 = T3.position;
        }

        private void DrawLine()
        {
            _line.positionCount = _numberOfSegments + 1;

            float step = 1.0f / _numberOfSegments;
            float t = 0.0f;

            for (int i = 0; i <= _numberOfSegments; ++i)
            {
                _line.SetPosition(i, _curve.GetPoint(t));
                t += step;
            }
        }

        private bool HasAllReferences()
        {
            return T0 && T1 && T2 && T3 && _line;
        }
    }
}