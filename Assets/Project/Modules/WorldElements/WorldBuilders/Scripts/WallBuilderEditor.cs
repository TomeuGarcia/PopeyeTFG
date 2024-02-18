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

        private WallBuilderConfig.EditorViewConfig _editorView;
        private readonly float _buttonSize = 0.2f;
        private readonly float _buttonPickSize = 0.25f;
        private float LineThickness => _editorView.LineThickness;


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

            if (!_wallBuilder.IsReadyForEditor())
            {
                return;
            }
            
            _points = _wallBuilder.Points;
            _editorView = _wallBuilder.EditorView;

            _handleTransform = _wallBuilder.transform;
            _handleRotation = Tools.pivotRotation == PivotRotation.Local
                ? _handleTransform.rotation
                : Quaternion.identity;


            Vector3[] drawSpacePoints = new Vector3[_points.Length];
            
            
            for (int i = 0; i < _points.Length; ++i)
            {
                Handles.color = _editorView.CornerButtonColor;
                drawSpacePoints[i] = DrawPoint(i);
                Handles.color = _editorView.CornerBlockColor;
                
                DrawBlock(_wallBuilder.CornerBlock, drawSpacePoints[i], Quaternion.identity);
            }


            for (int i = 1; i < _points.Length; ++i)
            {
                Vector3 previousPoint = drawSpacePoints[i - 1];
                Vector3 currentPoint = drawSpacePoints[i];

                Handles.color = _editorView.FillLineColor;
                Handles.DrawLine(previousPoint, currentPoint, LineThickness);

                if (_wallBuilder.FillBlock.Length < 0.01f)
                {
                    continue;
                }

                Vector3 previousToCurrent = currentPoint - previousPoint;
                float previousToCurrentDistance = previousToCurrent.magnitude;
                Vector3 previousToCurrentDirection = previousToCurrent / previousToCurrentDistance;
                
                Quaternion offsetRotation = Quaternion.LookRotation(previousToCurrentDirection, Vector3.up);
                
                Handles.color = _editorView.FillBlockColor;

                float lineLength = previousToCurrentDistance - _wallBuilder.CornerBlock.Length;
                float distanceCounter = _wallBuilder.CornerBlock.Length / 2 + _wallBuilder.FillBlock.Length / 2;

                for (; distanceCounter < lineLength; distanceCounter += _wallBuilder.FillBlock.Length)
                {
                    Vector3 fillPosition = previousPoint + (previousToCurrentDirection * distanceCounter);
                    DrawBlock(_wallBuilder.FillBlock, fillPosition, offsetRotation);
                }
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
                Handles.DrawLine(framePoints[i], framePoints[(i + 1) % framePoints.Length], LineThickness);
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