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
        
        private int _selectedIndex = -1;

        private WallBuilderConfig.EditorViewConfig _editorView;
        private float ButtonPickSize => _editorView.ButtonSize + 0.05f;


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            _wallBuilder = target as WallBuilder;
            _points = _wallBuilder.Points;

            GUILayout.Space(20);
            GUILayout.Label("BUTTONS");
            if (GUILayout.Button("Add Point"))
            {
                Undo.RecordObject(_wallBuilder, "Add Point");
                _wallBuilder.AddPoint();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            GUILayout.Space(15);
            if (GUILayout.Button("Center Around Pivot"))
            {
                Undo.RecordObject(_wallBuilder, "Center Around Pivot");
                _wallBuilder.CenterAroundPivot();
                EditorUtility.SetDirty(_wallBuilder);
            }
            
            
            GUILayout.Space(15);
            _wallBuilder.moveBy = EditorGUILayout.Vector2Field("Move By Amount", _wallBuilder.moveBy);
            Vector2Int pointsRange = EditorGUILayout.Vector2IntField("Selected Points Range", _wallBuilder.selectedPointsRange);
            pointsRange.x = Mathf.Max(pointsRange.x, 0);
            pointsRange.y = Mathf.Min(pointsRange.y, _points.Length);
            _wallBuilder.selectedPointsRange = pointsRange;
            if (GUILayout.Button("Move Points By Amount"))
            {
                Undo.RecordObject(_wallBuilder, "Move Selected Points Range By Amount");
                _wallBuilder.MoveBasePointsRangeByAmount(_wallBuilder.moveBy, pointsRange.x, pointsRange.y);
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
            
            
            for (int i = 0; i < _points.Length; ++i)
            {
                Handles.color = _editorView.CornerButtonColor;
                DrawPointButton(i, _points, ref _selectedIndex);
            }


            WallBuilder.SubPoints[] subPointsList = _wallBuilder.SubPointsList;
            foreach (WallBuilder.SubPoints subPoints in subPointsList)
            {
                for (int i = 0; i < subPoints.points.Length; ++i)
                {
                    Handles.color = _editorView.CornerButtonColor;
                    DrawPointButton(i, subPoints.points, ref _selectedIndexSubPoints);
                }
            }

        }

        private int _selectedIndexSubPoints = -1;


        private void DrawPointButton(int index, Vector3[] points, ref int selectedIndex)
        {
            Vector3 point = _handleTransform.TransformPoint(points[index]);
            
            if (Handles.Button(point, _handleRotation, _editorView.ButtonSize, ButtonPickSize, Handles.DotHandleCap))
            {
                selectedIndex = index;
            }

            if (selectedIndex != index) return;
            
            
            EditorGUI.BeginChangeCheck();
            point = Handles.DoPositionHandle(point, _handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_wallBuilder, "Move point");
                EditorUtility.SetDirty(_wallBuilder);
                points[index] = _handleTransform.InverseTransformPoint(point);
            }
        }

        
    }
}


#endif