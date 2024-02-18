#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

namespace Popeye.Modules.WorldElements.WorldBuilders
{
    [CustomEditor(typeof(WallBuilder))]
    public class WallBuilderEditor : Editor
    {
        private WallBuilder _wallBuilder;
        private Vector3[] _points;
        private Transform _handleTransform;
        private Quaternion _handleRotation;
        private int selectedIndex = -1;
        private readonly float _buttonSize = 0.05f;
        private readonly float _buttonPickSize = 0.07f;
        private readonly float _lineThickness = 2.0f;


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            _wallBuilder = target as WallBuilder;

            GUILayout.Space(20);
            if (GUILayout.Button("Add Point"))
            {
                Undo.RecordObject(_wallBuilder, "Add Point");
                _wallBuilder.AddPoint();
                EditorUtility.SetDirty(_wallBuilder);
            }
        }


        private void OnSceneGUI()
        {
            _wallBuilder = target as WallBuilder;
            _points = _wallBuilder.points;

            _handleTransform = _wallBuilder.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;


            Vector3[] drawSpacePoints = new Vector3[_points.Length];
            
            
            for (int i = 0; i < _points.Length; ++i)
            {
                Handles.color = Color.blue;
                drawSpacePoints[i] = DrawPoint(i);
                DrawBlock(_wallBuilder.cornerBlock, drawSpacePoints[i], Quaternion.identity);
            }
            
            
            for (int i = 1; i < _points.Length; ++i)
            {
                Handles.color = Color.red;
                Handles.DrawLine(drawSpacePoints[i - 1], drawSpacePoints[i], _lineThickness);

                Vector3 direction = (drawSpacePoints[i] - drawSpacePoints[i - 1]).normalized;
                Quaternion offsetRotation = Quaternion.LookRotation(direction, Vector3.up);
                
                
                Handles.color = Color.yellow;
                Vector3 center = Vector3.Lerp(drawSpacePoints[i - 1], drawSpacePoints[i], 0.5f);
                DrawBlock(_wallBuilder.fillBlock, center, offsetRotation);
            }
        }

        
        
        private Vector3 DrawPoint(int index)
        {
            Vector3 point = _handleTransform.TransformPoint(_points[index]);
            
            if (Handles.Button(point, _handleRotation, _buttonSize, _buttonPickSize, Handles.DotHandleCap))
            {
                selectedIndex = index;
            }

            if (selectedIndex != index) return point;
            
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_wallBuilder, "Move point");
                EditorUtility.SetDirty(_wallBuilder);
                _points[index] = _handleTransform.InverseTransformPoint(point);
            }

            return point;
        }

        private void DrawBlock(WallBuilder.Block block, Vector3 center, Quaternion rotation)
        {
            Vector3[] framePoints = block.ToFrame(center, rotation);
            for (int i = 0; i < framePoints.Length; ++i)
            {
                Handles.DrawLine(framePoints[i], framePoints[(i + 1) % framePoints.Length]);
            }

            Vector3 lookA = Vector3.Lerp(framePoints[0], framePoints[1], 0.5f);
            Vector3 lookB = Vector3.Lerp(framePoints[2], framePoints[3], 0.5f);
            Vector3 lookCenter = Vector3.Lerp(framePoints[0], framePoints[3], 0.5f);
            
            Handles.DrawLine(lookA, lookCenter);
            Handles.DrawLine(lookB, lookCenter);
        }
        
    }
}


#endif